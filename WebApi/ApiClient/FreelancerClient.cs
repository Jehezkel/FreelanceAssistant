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

    // Provides url to obtain auth code from frontend
    public string getAuthorizationUrl()
    {
        var authorizeRequest = new AuthorizeRequest(_freelancerConfig);
        var endpointWithParams = authorizeRequest.GetHttpRequest().RequestUri!.ToString();
        var baseUrl = new Uri(_freelancerConfig.AuthEndpoint);
        return new Uri(baseUrl, endpointWithParams).ToString();
    }
    public async Task<VerifyCodeResponse> VerifyCode(string code) =>
        await ExecuteRequest<VerifyCodeResponse, VerifyCodeRequest, VerifyCodeInput>(new VerifyCodeInput(_freelancerConfig, code));
    public async Task<List<ProjectResponse>> FetchProjects(string access_token, ActiveProjectsInput? input = null) =>
         await ExecuteRequest<List<ProjectResponse>, ActiveProjectsRequest, ActiveProjectsInput>(input, access_token);
    public async Task<SelfInformationResponse> GetUser(string access_token) =>
            await ExecuteRequest<SelfInformationResponse, SelfInformationRequest>(access_token);
    public async Task<List<JobResponse>> GetJobs(string access_token, JobsInput? input = null) =>
            await ExecuteRequest<List<JobResponse>, JobsRequest, JobsInput>(input, access_token);
    public async Task<CreateBidResponse> CreateBid(string access_token, CreateBidInput input) => 
            await ExecuteRequest<CreateBidResponse, CreateBidRequest, CreateBidInput>(input, access_token);
    
    private async Task<ResponseClass> ExecuteRequest<ResponseClass, ReqCreatorClass, InputClass>(InputClass? inputObject, string? accessToken = null)
        where ReqCreatorClass : BaseRequest<InputClass>, new()
        where ResponseClass : new()
        where InputClass : new()
    {
        var reqCreatorObject = new ReqCreatorClass();
        reqCreatorObject.RequestInputObject = inputObject;
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