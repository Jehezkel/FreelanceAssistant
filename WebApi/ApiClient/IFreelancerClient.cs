using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.Responses;
using WebApi.FreelanceQueries;

namespace WebApi.ApiClient
{
    public interface IFreelancerClient
    {
        Task<GenericResponse> AddJobsAsync(string access_token, AddJobsInput input);
        Task<CreateBidResponse> CreateBidAsync(string access_token, CreateBidInput input);
        Task<List<ProjectResponse>> FetchProjectsAsync(string access_token, ActiveProjectsInput? input = null);
        string getAuthorizationUrl();
        Task<List<BidResponse>> GetBidsAsync(string access_token);
        Task<GenericResponse> BidAction(string access_token, BidActionInput input);
        Task<List<JobResponse>> GetJobsAsync(string access_token, JobsInput? input = null);
        Task<SelfInformationResponse> GetSelfInformationAsync(string access_token);
        Task<VerifyCodeResponse> VerifyCodeAsync(string code);
    }
}