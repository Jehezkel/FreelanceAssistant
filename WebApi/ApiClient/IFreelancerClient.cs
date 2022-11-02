using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.Responses;
using WebApi.FreelanceQueries;
using WebApi.Models;

namespace WebApi.ApiClient;

public interface IFreelancerClient
{
    //Task<IEnumerable<ActiveProjectsResponse>> FetchProjects(string access_token);
    string getAuthorizationUrl();
    Task<VerifyCodeResponse> VerifyCode(string code);
    Task<IEnumerable<ProjectResponse>> FetchProjects(string accessToken, ActiveProjectsInput? input=null);
    Task<SelfInformationResponse> GetUser(string accessToken);
    Task<IReadOnlyList<JobResponse>> GetJobs(string accessToken, JobsInput? input = null);
    Task<CreateBidResponse> CreateBid(FLApiToken fLApiToken, CreateBidInput? input = null);

}
