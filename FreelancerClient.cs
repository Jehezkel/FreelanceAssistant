using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApi;

public class FreelancerClient
{
    // private readonly IConfiguration _config;
    private FreelancerConfig _freelancerConfig = new FreelancerConfig();
    private readonly ILogger<FreelancerClient> _logger;

    public FreelancerClient(IConfiguration config, ILogger<FreelancerClient> logger)
    {
        config.GetSection("Freelancer").Bind(_freelancerConfig);
        _logger = logger;
    }
    public void AuthorizationRequest()
    {
        Dictionary<string, string?> QueryParams = new();

        QueryParams.Add("response_type", "code");
        QueryParams.Add("client_id", _freelancerConfig.clientId);
        QueryParams.Add("redirec_uri", _freelancerConfig.redirectUri);
        QueryParams.Add("scope", "basic");
        QueryParams.Add("advanced_scopes", "2");
        var test = QueryHelpers.AddQueryString(_freelancerConfig.authEndpoint, QueryParams);
        _logger.LogDebug("Url that will be used: {0}", test);
        // { 0}?response_type = code'
        // '&client_id={1}&redirect_uri={2}'
        // '&scope=basic&prompt={3}'
        // '&advanced_scopes={4}'.format(
        //     oauth_uri, client_id, redirect_uri, prompt, advanced_scopes
        // )
    }
}