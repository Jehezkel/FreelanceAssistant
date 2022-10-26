using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DAL;
using WebApi.Models;
using WebApi.ApiClient;

namespace WebApi.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class FreelanceController : ControllerBase
{
    private readonly ILogger<FreelanceController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly FLDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFreelancerClient _flClient;

    public FreelanceController(ILogger<FreelanceController> logger, IHttpContextAccessor contextAccessor, FLDbContext dbContext, UserManager<AppUser> userManager, IFreelancerClient flClient)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
        _dbContext = dbContext;
        _userManager = userManager;
        _flClient = flClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetServiceConfigState()
    {

        var currentUser = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
        var flApiToken = await _dbContext.FLApiTokens.Where(t => t.User == currentUser).FirstOrDefaultAsync();
        if (flApiToken is null)
        {
            return Ok(new { authUrl = _flClient.getAuthorizationUrl() });
        }
        return Ok();
    }
    [HttpPost]
    [Route("VerifyCode")]
    public async Task<IActionResult> VerifyCode(string code)
    {
        var verResult = await _flClient.VerifyCode(code);
        if (verResult.AccessToken is not null)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
            var flApiToken = new FLApiToken
            {
                AccessToken = verResult.AccessToken,
                RefreshToken = verResult.RefreshToken!,
                User = user,
                UserID = user.Id,
            };
            _logger.LogInformation("Saving token: {0}", flApiToken);
            _dbContext.FLApiTokens.Add(flApiToken);
            await _dbContext.SaveChangesAsync();
        }

        return Ok();
    }
    [HttpGet]
    [Route("projects")]
    public async Task<IActionResult> GetProjects()
    {
        var token = await GetAccessTokenAsync();
        return Ok(await _flClient.FetchProjects(token));
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var currentUser = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
        var token = await _dbContext.FLApiTokens.FirstOrDefaultAsync(t => t.UserID == currentUser.Id);
        return token.AccessToken ?? "";
    }
}