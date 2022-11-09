using Microsoft.Extensions.Options;
using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.Requests;
using WebApi.ApiClient.Responses;
using WebApi.FreelanceQueries;
using WebApi.Models;

namespace WebApi.ApiClient;

public class FreelancerClient : IFreelancerClient
{
    private FreelancerConfig _freelancerConfig;
    private readonly ILogger<FreelancerClient> _logger;
    private readonly HttpClient _httpClient;
    public FreelancerClient(IOptions<FreelancerConfig> options, ILogger<FreelancerClient> logger,
                            HttpClient httpClient)
    {
        _freelancerConfig = options.Value;
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_freelancerConfig.BaseAddress);
    }

    /// <summary>
    /// Helper method to provide frontend Authorization url
    /// </summary>
    /// <returns>authorization url of freelance</returns>
    public string getAuthorizationUrl()
    {
        var authorizeRequest = new AuthorizeRequest(_freelancerConfig);
        var endpointWithParams = authorizeRequest.GetHttpRequest().RequestUri!.ToString();
        var baseUrl = new Uri(_freelancerConfig.AuthEndpoint);
        return new Uri(baseUrl, endpointWithParams).ToString();
    }


    /// <summary>
    /// Verify code received from freelance callback
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<VerifyCodeResponse> VerifyCodeAsync(string code)
    {
        var baseAuthUrl = new Uri(_freelancerConfig.AuthEndpoint);
        var verifyCodeInput = new VerifyCodeInput(_freelancerConfig, code);
        var reqCreator = new VerifyCodeRequest();
        reqCreator.RequestInputObject = verifyCodeInput;
        var httpRequest = reqCreator.GetHttpRequest();
        var relativReqUri = httpRequest.RequestUri!.ToString();
        httpRequest.RequestUri = new Uri(baseAuthUrl, relativReqUri);
        var response = await _httpClient.SendAsync(httpRequest);
        if (response.IsSuccessStatusCode)
        {
            var resultObject = await response.Content.ReadFromJsonAsync<VerifyCodeResponse>();
            return resultObject;
        }
        var errorResult = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        if (errorResult is not null)
        {
            errorResult.StatusCode = response.StatusCode;
            throw new FLApiClientException(errorResult);
        }
        else
        {
            throw new FLApiClientException(await response.Content.ReadAsStringAsync());
        }

    }
        //this must go to different baseurl - change usage from generic to specific function
        //await ExecuteRequest<VerifyCodeResponse, VerifyCodeRequest, VerifyCodeInput>(new VerifyCodeInput(_freelancerConfig, code));


    /// <summary>
    /// Fetch active prjects - pass filters via activeprojectsinput
    /// </summary>
    /// <param name="access_token">Freelance access token</param>
    /// <param name="input">possible filtering options</param>
    /// <returns>List of projects</returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<ActiveProjectsResponse> FetchProjectsAsync(string access_token, ActiveProjectsInput? input = null) =>
         await ExecuteRequest<ActiveProjectsResponse, ActiveProjectsRequest, ActiveProjectsInput>(input, access_token);


    /// <summary>
    /// Get self user info - needed for bidder_id
    /// </summary>
    /// <param name="access_token">Freelance access token</param>
    /// <returns cref="SelfInformationResponse"></returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<SelfInformationResponse> GetSelfInformationAsync(string access_token) =>
            await ExecuteRequest<SelfInformationResponse, SelfInformationRequest>(access_token);


    /// <summary>
    /// Get list of jobs
    /// </summary>
    /// <param name="access_token"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<List<JobResponse>> GetJobsAsync(string access_token, JobsInput? input = null) =>
            await ExecuteRequest<List<JobResponse>, JobsRequest, JobsInput>(input, access_token);


    /// <summary>
    /// Add job ids to currently logged user
    /// </summary>
    /// <param name="access_token"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<GenericResponse> AddJobsAsync(string access_token, AddJobsInput input) =>
            await ExecuteRequest<GenericResponse, AddJobsRequest, AddJobsInput>(input, access_token);


    /// <summary>
    /// Create bid for project id
    /// </summary>
    /// <param name="access_token"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<CreateBidResponse> CreateBidAsync(string access_token, CreateBidInput input) =>
            await ExecuteRequest<CreateBidResponse, CreateBidRequest, CreateBidInput>(input, access_token);


    /// <summary>
    /// List all bids
    /// </summary>
    /// <param name="access_token"></param>
    /// <returns></returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<List<BidResponse>> GetBidsAsync(string access_token) =>
        await ExecuteRequest<List<BidResponse>, ListBidsRequest>(access_token);

    /// <summary>
    /// Perform actions (retract/sponsor/highlight) on bids
    /// </summary>
    /// <param name="access_token"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="FLApiClientException"></exception>
    public async Task<GenericResponse> BidAction(string access_token, BidActionInput input) =>
        await ExecuteRequest<GenericResponse, BidActionRequest, BidActionInput>(input, access_token);

    private async Task<ResponseClass> ExecuteRequest<ResponseClass, ReqCreatorClass, InputClass>(InputClass? inputObject, string? accessToken = null)
        where ReqCreatorClass : BaseRequest<InputClass>, new()
        where ResponseClass : new()
        where InputClass : new()
    {
        var reqCreatorObject = new ReqCreatorClass();
        reqCreatorObject.RequestInputObject = inputObject;
        _logger.LogInformation("Input object: {0}", inputObject);
        var httpRequest = reqCreatorObject.GetHttpRequest();
        return await SendHttpReqMessage<ResponseClass>(httpRequest, accessToken);
    }

    private async Task<ResponseClass> ExecuteRequest<ResponseClass, ReqCreatorClass>(string? accessToken = null)
       where ReqCreatorClass : BaseRequest, new()
       where ResponseClass : new()
    {
        var reqCreatorObject = new ReqCreatorClass();
        var httpRequest = reqCreatorObject.GetHttpRequest();
        return await SendHttpReqMessage<ResponseClass>(httpRequest, accessToken);
    }

    private async Task<ResponseClass> SendHttpReqMessage<ResponseClass>(HttpRequestMessage requestMessage, string? accessToken)
        where ResponseClass : new()
    {
        if (accessToken is not null)
        {
            _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", accessToken);
        }

        var response = await _httpClient.SendAsync(requestMessage);
        if (response.IsSuccessStatusCode)
        {
            var resultObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<ResponseClass>>();
            return resultObject!.Result;
        }
        var errorResult = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        if (errorResult is not null)
        {
            errorResult.StatusCode = response.StatusCode;
            throw new FLApiClientException(errorResult);
        }
        else
        {
            throw new FLApiClientException(await response.Content.ReadAsStringAsync());
        }
    }


}