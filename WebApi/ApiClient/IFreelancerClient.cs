using WebApi.ApiClient.Responses;
using WebApi.Models;

namespace WebApi.ApiClient;

public interface IFreelancerClient
{
    Task<IEnumerable<ActiveProjectsResponse>> FetchProjects(string access_token);
    string getAuthorizationUrl();
    Task<AccessTokenResponse> VerifyCode(string code);
    Task<IEnumerable<ActiveProjectsResponse>> NewFetchProjects(string access_token);

}
