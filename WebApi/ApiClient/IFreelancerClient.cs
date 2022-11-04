using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.Responses;
using WebApi.FreelanceQueries;
using WebApi.Models;

namespace WebApi.ApiClient
{
    public interface IFreelancerClient
    {
        Task<CreateBidResponse> CreateBid(string fLApiToken, CreateBidInput input);
        Task<List<ProjectResponse>> FetchProjects(string access_token, ActiveProjectsInput? input = null);
        string getAuthorizationUrl();
        Task<List<JobResponse>> GetJobs(string access_token, JobsInput? input = null);
        Task<SelfInformationResponse> GetUser(string access_token);
        Task<VerifyCodeResponse> VerifyCode(string code);
    }
}