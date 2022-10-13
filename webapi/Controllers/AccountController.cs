using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Account;
using WebApi.DAL;
using WebApi.Models;
using WebApi.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace WebApi.Controllers;
[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly FLDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(FLDbContext context, ILogger<AccountController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, TokenService tokenService)
    {
        _context = context;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
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
        var result = await _userManager.CreateAsync(new AppUser
        {
            Email = request.Email,
            UserName = request.UserName
        }, request.Password);
        if (result == IdentityResult.Success)
        {

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
            // var loginResult = new LoginResult{
            //     AccessToken = 
            // }
            return Ok();
        }
        if (loginResult == SignInResult.Failed)
            return Ok("Password incorrect");
        return Problem("Unknown error occured on login");
    }
    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        return Ok();
    }

}