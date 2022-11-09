using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ApiClient;
using System.Security.Claims;
using WebApi.Mapping;
using WebApi.Repositories;
using WebApi.ApiClient.RequestInputs;
using WebApi.Models;

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
        var verResult = await _flClient.VerifyCodeAsync(code);
        if (verResult.AccessToken is not null)
        {
            var currentUserID = GetUserId();
            var flApiToken = verResult.ToFLApiToken(currentUserID);
            var flUser = await _flClient.GetSelfInformationAsync(flApiToken.AccessToken);
            _logger.LogInformation("Received user info: {0}", flUser);
            flApiToken.FLUserID = flUser.UserId;
            _logger.LogInformation("Saving token: {0}", flApiToken);
            await _fLApiTokenRepository.CreateFLApiToken(flApiToken);
            return Ok();
        }
        throw new Exception("Failed to verify provided code with Freelance");
    }
    [HttpGet]
    [Route("Projects")]
    public async Task<IActionResult> GetProjects()
    {
        var accessToken = await GetAccessTokenAsync();
        if (accessToken is not null)
        {
            return Ok(await _flClient.FetchProjectsAsync(accessToken));
        }
        return Problem(detail: "Configure integration token first! Token for Freelancer  not found",
                        title: "Integration not configured");
    }
    [HttpPost]
    [Route("CreateBid")]
    public async Task<IActionResult> CreateBid([FromQuery] int projectId, [FromBody] CreateBidInput body)
    {
        var token = await GetFlApiTokenForCurrentUser();
        if (token is not null)
        {
            body.BidderId = token.FLUserID;
            return Ok(await _flClient.CreateBidAsync(token.AccessToken, body));
        }
        return Problem(detail: "Configure integration token first! Token for Freelancer  not found",
                        title: "Integration not configured");
    }

    [HttpGet]
    [Route("GetBids")]
    public async Task<IActionResult> GetBids()
    {
        var accessToken = await GetAccessTokenAsync();
        if (accessToken is not null)
        {
            await _flClient.GetBidsAsync(accessToken);
            return Ok();
        }
        return Problem(detail: "Configure integration token first! Token for Freelancer  not found",
                        title: "Integration not configured");
    }

    [HttpPost]
    [Route("BidAction/{bidId:int}")]
    public async Task<IActionResult> BidAction([FromRoute]int bidId, [FromBody]BidActionInput body)
    {
        body.BidId = bidId; 
        
        var accessToken = await GetAccessTokenAsync();
        if (accessToken is not null)
        {
            await _flClient.BidAction(accessToken, body);
            return Ok();
        }
        return Problem(detail: "Configure integration token first! Token for Freelancer  not found",
                        title: "Integration not configured");
    }

    [HttpPost]
    [Route("AddJobs")]
    public async Task<IActionResult> AddJobs([FromBody] AddJobsInput body)
    {
        var accessToken = await GetAccessTokenAsync();
        if (accessToken is not null)
        {
            await _flClient.AddJobsAsync(accessToken, body);
            return Ok();
        }
        return Problem(detail: "Configure integration token first! Token for Freelancer  not found",
                        title: "Integration not configured");
    }
    [HttpGet]
    [Route("GetJobs")]
    public async Task<IActionResult> GetJobs([FromQuery]JobsInput jobsInput)
    {
        var accessToken = await GetAccessTokenAsync();
        if (accessToken is not null)
        {
            await _flClient.GetJobsAsync(accessToken, jobsInput);
            return Ok();
        }
        return Problem(detail: "Configure integration token first! Token for Freelancer  not found",
                        title: "Integration not configured");
    }
    private async Task<FLApiToken?> GetFlApiTokenForCurrentUser()
    {
        var currentUserID = GetUserId();
        var accessTokenValue = await _fLApiTokenRepository.GetFLApiToken(currentUserID);
        return accessTokenValue;
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