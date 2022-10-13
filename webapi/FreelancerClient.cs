using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi;

public class FreelancerClient
{
    private FreelancerConfig _freelancerConfig;
    private readonly ILogger<FreelancerClient> _logger;
    private readonly HttpClient _httpClient;
    private AccessTokenResponse TokenResponse = new AccessTokenResponse();

    public FreelancerClient(IOptions<FreelancerConfig> options, ILogger<FreelancerClient> logger)
    {
        _freelancerConfig = options.Value;
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_freelancerConfig.BaseAddress)
        };

    }
    public bool IsAuthorized
    {
        get { return !String.IsNullOrEmpty(this.TokenResponse.access_token); }
        private set { }
    }
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
    public async void verifyCode(string code)
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
        _logger.LogDebug("Test convert request {0}", JsonSerializer.Serialize(req));
        _logger.LogDebug("Destination URL: {0}", response.RequestMessage.RequestUri);
        _logger.LogDebug("Request headers: {0}", JsonSerializer.Serialize(response.RequestMessage.Headers));

        _logger.LogDebug("Status code: {0}", response.StatusCode);
        _logger.LogDebug("Response content: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
        if (response.Content is not null)
        {
            this.TokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
            _logger.LogDebug("Success: {0}", this.TokenResponse.access_token);
        }
        // return response.Result.Content;
    }
    public async Task<IEnumerable<Project>> fetchProjects()
    {
        var activeUri = new Uri(new Uri(_freelancerConfig.BaseAddress), new Uri("projects/0.1/projects/active", UriKind.Relative));
        _httpClient.DefaultRequestHeaders.Add("freelancer-oauth-v1", TokenResponse.access_token);
        var responseContent = await _httpClient.GetFromJsonAsync<ProjectsResponse>(activeUri);
        if (responseContent.status == "success")
        {
            return responseContent.result.projects;
        }
        return Enumerable.Empty<Project>();
    }
}