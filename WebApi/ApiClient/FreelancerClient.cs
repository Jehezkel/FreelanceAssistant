using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.Requests;
using WebApi.ApiClient.Responses;
using WebApi.Handlers;

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
        string authUri = new Uri(new Uri(_freelancerConfig.AuthEndpoint), new Uri("authorize", UriKind.Relative)).ToString();
        Dictionary<string, string?> QueryParams = new();
        QueryParams.Add("response_type", "code");
        QueryParams.Add("client_id", _freelancerConfig.ClientID);
        QueryParams.Add("redirect_uri", _freelancerConfig.RedirectUri);
        QueryParams.Add("scope", "basic");
        QueryParams.Add("advanced_scopes", "2");
        var authUrl = QueryHelpers.AddQueryString(authUri, QueryParams);
        _logger.LogDebug("Url that will be used: {0}", authUrl);
        return authUrl;
    }
    //Verify code obtained by user from frontend and get AccessToken
    public async Task<AccessTokenResponse> VerifyCode_Old(string code)
    {
        var tokenUri = new Uri(new Uri(_freelancerConfig.AuthEndpoint), new Uri("token", UriKind.Relative));

        var verifyCodeParams = new Dictionary<string, string>();
        verifyCodeParams.Add("grant_type", "authorization_code");
        verifyCodeParams.Add("code", code);
        verifyCodeParams.Add("client_id", _freelancerConfig.ClientID);
        verifyCodeParams.Add("client_secret", _freelancerConfig.ClientSecret);
        verifyCodeParams.Add("redirect_uri", _freelancerConfig.RedirectUri);
        HttpClient authClient = new HttpClient();

        HttpRequestMessage req = new HttpRequestMessage();
        req.Content = new FormUrlEncodedContent(verifyCodeParams);
        req.Method = HttpMethod.Post;
        req.RequestUri = tokenUri;

        var response = await authClient.SendAsync(req);
        // _logger.LogDebug("Test convert request {0}", JsonSerializer.Serialize(req));
        // _logger.LogDebug("Destination URL: {0}", response!.RequestMessage!.RequestUri);
        // _logger.LogDebug("Request headers: {0}", JsonSerializer.Serialize(response.RequestMessage.Headers));

        // _logger.LogDebug("Status code: {0}", response.StatusCode);
        // _logger.LogDebug("Response content: {0}", await response.Content.ReadAsStringAsync());
        // response.EnsureSuccessStatusCode();
        if (response.Content is not null)
        {
            var result = await response!.Content.ReadFromJsonAsync<AccessTokenResponse>() ?? new AccessTokenResponse();
            _logger.LogDebug("Success: {0}", result);
            return result;
        }
        throw new Exception(String.Format("Verification for code {0} failed", code));
        // return response.Result.Content;
    }
    public async Task<AccessTokenResponse> VerifyCode(string code)
    {
        var request = new VerifyCodeRequest();
        var inputObj = new VerifyCodeInput(_freelancerConfig);
        inputObj.Code = code;
        request.RequestInputObject = inputObj;

        HttpClient authClient = new HttpClient { BaseAddress = new Uri(_freelancerConfig.AuthEndpoint) };
        var httpRequest = request.GetHttpRequest();
        var response = await authClient.SendAsync(httpRequest);
        if(response.Content is not null && response.IsSuccessStatusCode)
        {
            var result = await response!.Content.ReadFromJsonAsync<AccessTokenResponse>() ?? new AccessTokenResponse();
            _logger.LogDebug("Success: {0}", result);
            return result;
        }
        throw new Exception(String.Format("Verification for code {0} failed", code));
    }
    public async Task<IEnumerable<ActiveProjectsResponse>> FetchProjects(string access_token)
    {
        var activeUri = new Uri(new Uri(_freelancerConfig.BaseAddress), new Uri("projects/0.1/projects/active", UriKind.Relative));
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", access_token);
        var responseContent = await _httpClient.GetFromJsonAsync<ProjectSearchResponse>(activeUri) ?? new ProjectSearchResponse();
        
        if (responseContent.Status == "success")
        {
            return responseContent.Result.Projects;
        }
        return Enumerable.Empty<ActiveProjectsResponse>();
    }
    public async Task<IEnumerable<ActiveProjectsResponse>> NewFetchProjects(string access_token)
    {
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", access_token);
        var projectRequest = new ActiveProjectsRequest();
        projectRequest.RequestInputObject.MinPrice = 40;


        var httpRequest = projectRequest.GetHttpRequest();
        var result = await _httpClient.SendAsync(httpRequest);
        var responseContent = await result.Content.ReadFromJsonAsync<ProjectSearchResponse>();
        return responseContent.Result.Projects;
        /*if (responseContent.Status == "success")
        {
            return responseContent.Result.Projects;
        }*/
        //return Enumerable.Empty<ProjectResponse>();
    }
}