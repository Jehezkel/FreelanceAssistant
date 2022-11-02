using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.Requests;
using WebApi.ApiClient.Responses;
using WebApi.FreelanceQueries;
using WebApi.Handlers;
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
    //Verify code obtained by user from frontend and get AccessToken
    public async Task<VerifyCodeResponse> VerifyCode(string code)
    {
        var request = new VerifyCodeRequest(_freelancerConfig, code);

        HttpClient authClient = new HttpClient { BaseAddress = new Uri(_freelancerConfig.AuthEndpoint) };
        var httpRequest = request.GetHttpRequest();
        var response = await authClient.SendAsync(httpRequest);
        if (response.Content is not null && response.IsSuccessStatusCode)
        {
            var result = await response!.Content.ReadFromJsonAsync<VerifyCodeResponse>() ?? new VerifyCodeResponse();
            _logger.LogDebug("Success: {0}", result);
            return result;
        }
        var respContent = await response.Content!.ReadAsStringAsync();
        throw new FLApiClientException($"Authorization for code {code} failed with message:\n {respContent}");
    }
    public async Task<IEnumerable<ProjectResponse>> FetchProjects(string access_token, ActiveProjectsInput? input = null)
    {
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", access_token);
        var projectRequest = new ActiveProjectsRequest();
        if (input is not null)
        {
            projectRequest.RequestInputObject = input;
        }

        var httpRequest = projectRequest.GetHttpRequest();
        var result = await _httpClient.SendAsync(httpRequest);
        var response = await result.Content.ReadFromJsonAsync<ResponseWrapper<ActiveProjectsResponse>>();
        if (response is not null && response.Status == "success")
        {
            return response.Result.Projects;
        }

        var exceptionMessage = await result.Content.ReadAsStringAsync();
        throw new FLApiClientException(exceptionMessage);
    }

    public async Task<SelfInformationResponse> GetUser(string access_token)
    {
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", access_token);

        var selfRequest = new SelfInformationRequest();
        var httpRequest = selfRequest.GetHttpRequest();
        var result = await _httpClient.SendAsync(httpRequest);
        var response = await result.Content.ReadFromJsonAsync<ResponseWrapper<SelfInformationResponse>>();
        if (response is not null && response?.Status == "success")
        {
            return response.Result;
        }
        var exceptionMessage = await result.Content.ReadAsStringAsync();
        throw new FLApiClientException(exceptionMessage);
    }

    public async Task<IReadOnlyList<JobResponse>> GetJobs(string access_token, JobsInput? input = null)
    {
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", access_token);
        var requestCreator = new JobsRequest();
        if (input is not null)
        {
            requestCreator.RequestInputObject = input;
        }
        var request = requestCreator.GetHttpRequest();
        var result = await _httpClient.SendAsync(request);
        var response = await result.Content.ReadFromJsonAsync<ResponseWrapper<List<JobResponse>>>();
        if (response is not null && response?.Status == "success")
        {
            return response.Result;
        }
        var exceptionMessage = await result.Content.ReadAsStringAsync();
        throw new FLApiClientException(exceptionMessage);
    }

    public async Task<CreateBidResponse> CreateBid(FLApiToken fLApiToken, CreateBidInput? input = null)
    {
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", fLApiToken.AccessToken);
        var requestCreator = new CreateBidRequest();
        if(input is not null)
        {
            requestCreator.RequestInputObject = input;
           
        }
        requestCreator.RequestInputObject.BidderId = fLApiToken.FLUserID;
        var request = requestCreator.GetHttpRequest();
        var result = await _httpClient.SendAsync(request);
        var response = await result.Content.ReadFromJsonAsync<ResponseWrapper<CreateBidResponse>>();
        if (response is not null && response?.Status == "success")
        {
            return response.Result;
        }
        var exceptionMessage = await result.Content.ReadAsStringAsync();
        throw new FLApiClientException(exceptionMessage);
    }


}