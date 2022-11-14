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
    private readonly ITokenService _tokenService;
    private readonly MailService _mailService;

    public AccountController(FLDbContext context, ILogger<AccountController> logger, SignInManager<AppUser> signInManager, 
        UserManager<AppUser> userManager, 
        ITokenService tokenService, MailService mailService)
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
            var activationToken = await _tokenService.CreateActivationTokenAsync(user.Id);
            _logger.LogInformation("Sending activation email to {0} with code {1}", user.Email, activationToken);
            _ = _mailService.SendActivationMail(user, activationToken);
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
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user.Id);
            var result = new LoginResult
            {
                UserName = user.UserName,
                RefreshToken = refreshToken,
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
        var token = await _context.UserTempTokens
            .Include(t => t.User)
            .Where(t => t.TokenValue == refreshToken && t.Type == TokenType.Refresh)
            .FirstOrDefaultAsync();
        if (token is null)
        {
            _logger.LogError($"Refresh token with value {refreshToken} not found!");
            return NotFound($"Refresh token with value {refreshToken} not found!");
        }
        var user = token.User;
        var userRoles = await _userManager.GetRolesAsync(user);
        var newRefreshTokenValue = await _tokenService.CreateRefreshTokenAsync(token.UserID);
        var refreshResult = new RefreshResult
        {
            RefreshToken = newRefreshTokenValue,
            AccessToken = _tokenService.GenerateAccessToken(user, userRoles)
        };

        return Ok(refreshResult);
    }
    [HttpGet]
    [Route("ActivateAccount")]
    public async Task<IActionResult> ActivateAccount([FromQuery] string tokenValue)
    {
        var userToken = await _context.UserTempTokens.Include(t => t.User)
            .Where(t => t.TokenValue == tokenValue && t.Type == TokenType.Activation)
            .FirstOrDefaultAsync();
        if (userToken is null)
        {
            return NotFound("Activation code not found.");
        }
        userToken.User.EmailConfirmed = true;
        _context.UserTempTokens.Remove(userToken);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPost]
    [Route("ResetPasswordRequest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequest request)
    {

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            _logger.LogError("No user found with mail {0}", request.Email);
            return NotFound("No user registered with specified email.");
        }
        // _mailService.SendActivationMail
        return Ok();
    }

    

}