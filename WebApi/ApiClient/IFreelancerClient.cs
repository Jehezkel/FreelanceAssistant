using WebApi.ApiClient.Responses;
using WebApi.FreelanceQueries;
using WebApi.Models;

namespace WebApi.ApiClient;

public interface IFreelancerClient
{
    //Task<IEnumerable<ActiveProjectsResponse>> FetchProjects(string access_token);
    string getAuthorizationUrl();
    Task<VerifyCodeResponse> VerifyCode(string code);
    Task<IEnumerable<ProjectResponse>> FetchProjects(string access_token, ActiveProjectsInput? input=null);
    Task<UserResponse> GetUser(string access_token);

}
