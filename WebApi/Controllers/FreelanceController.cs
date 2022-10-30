using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ApiClient;
using System.Security.Claims;
using WebApi.Mapping;
using WebApi.Repositories;

namespace WebApi.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class FreelanceController : ControllerBase
{
    private readonly ILogger<FreelanceController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IFreelancerClient _flClient;
    private readonly IFLApiTokenRepository _fLApiTokenRepository;

    public FreelanceController(ILogger<FreelanceController> logger, IHttpContextAccessor contextAccessor, IFreelancerClient flClient, IFLApiTokenRepository fLApiTokenRepository)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
        _flClient = flClient;
        _fLApiTokenRepository = fLApiTokenRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetServiceConfigState()
    {

        // var currentUser = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
        var flApiToken = await GetAccessTokenAsync();
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
            var currentUserID = GetUserId();
            var flApiToken = verResult.ToFLApiToken(currentUserID);
            _logger.LogInformation("Saving token: {0}", flApiToken);
            await _fLApiTokenRepository.CreateFLApiToken(flApiToken);
        }
        return Ok();
    }
    [HttpGet]
    [Route("Projects")]
    public async Task<IActionResult> GetProjects()
    {
        var token = await GetAccessTokenAsync();
        return Ok(await _flClient.NewFetchProjects(token!));
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        var currentUserID = GetUserId();
        var accessTokenValue = await _fLApiTokenRepository.GetAccessToken(currentUserID);
        return accessTokenValue;
    }
    private string GetUserId()
    {
        return _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}