using WebApi.ApiClient.Responses;
using WebApi.Models;

namespace WebApi.ApiClient;

public interface IFreelancerClient
{
    Task<IEnumerable<ProjectResponse>> FetchProjects(string access_token);
    string getAuthorizationUrl();
    Task<AccessTokenResponse> VerifyCode(string code);
}
