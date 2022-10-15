using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Account;
using WebApi.DAL;
using WebApi.Models;
using WebApi.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly FLDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly MailService _mailService;

    public AccountController(FLDbContext context, ILogger<AccountController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, TokenService tokenService, MailService mailService)
    {
        _context = context;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _mailService = mailService;
    }
    [HttpPost]
    [Route("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null)
        {
            return Conflict("Email already in use - please recover password or use other email address");
        }
        user = new AppUser
        {
            Email = request.Email,
            UserName = request.UserName
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result == IdentityResult.Success)
        {
            var activationToken = await GenerateTempToken(user, TokenType.Activation);
            _logger.LogInformation("Sending activation email to {0} with code {1}", user.Email, activationToken.TokenValue);
            await _mailService.SendActivationMail(user, activationToken.TokenValue);
            return Ok();

        }
        return Problem(title: "Unknown error during registration", detail: result.Errors.ToString());
    }
    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResult))]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return NotFound("Email not found! Check email and register if needed.");
        var loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (loginResult == SignInResult.Success)
        {
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var refreshToken = await GenerateTempToken(user, TokenType.Refresh);
            var result = new LoginResult
            {
                UserName = user.UserName,
                RefreshToken = refreshToken.TokenValue,
                AccessToken = _tokenService.GenerateAccessToken(user, userRoles),
                UserRoles = userRoles
            };
            return Ok(result);
        }
        if (loginResult == SignInResult.Failed)
            return Ok("Password incorrect");
        return Problem("Unknown error occured on login");
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var token = await _context.UserTokens.Include(t => t.User).Where(t => t.TokenValue == refreshToken && t.Type == TokenType.Refresh).FirstOrDefaultAsync();
        if (token is null)
        {
            _logger.LogError($"Refresh token with value {refreshToken} not found!");
            return NotFound($"Refresh token with value {refreshToken} not found!");
        }
        var user = token.User;
        var userRoles = await _userManager.GetRolesAsync(user);
        var newRefreshToken = await GenerateTempToken(token.User, TokenType.Refresh);
        var refreshResult = new RefreshResult
        {
            RefreshToken = newRefreshToken.TokenValue,
            AccessToken = _tokenService.GenerateAccessToken(user, userRoles)
        };

        return Ok(refreshResult);
    }
    [HttpGet]
    [Route("ActivateAccount/{tokenValue}")]
    public async Task<IActionResult> ActivateAccount([FromRoute] string tokenValue)
    {
        var userToken = await _context.UserTokens.Include(t => t.User).Where(t => t.TokenValue == tokenValue && t.Type == TokenType.Activation).FirstOrDefaultAsync();
        if (userToken is null)
        {
            return NotFound("Activation code not found.");
        }
        userToken.User.EmailConfirmed = true;
        _context.UserTokens.Remove(userToken);
        await _context.SaveChangesAsync();
        return Ok();
    }
    private async Task<UserToken> GenerateTempToken(AppUser user, TokenType tokenType)
    {
        var token = await _context.UserTokens.Where(t => t.User == user && t.Type == tokenType).FirstOrDefaultAsync();
        var tokenVal = tokenType == TokenType.Refresh ? _tokenService.GenerateRefreshToken() : Guid.NewGuid().ToString();
        if (token is null)
        {
            _logger.LogDebug("New token");
            token = new UserToken
            {
                User = user,
                TokenValue = tokenVal,
                Type = tokenType
            };
            _context.UserTokens.Add(token);
        }
        if (token is not null)
        {
            _logger.LogDebug("Updating token value");
            token.TokenValue = tokenVal;
        }
        token.CreateDate = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        return token!;
    }

}