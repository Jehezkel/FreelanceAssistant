using WebApi.FreelanceQueries;

namespace WebApi.ApiClient.Requests;
public  class ProjectRequest : BaseRequest<ProjectsFilter>
{
    public override string EndpointUrl => "projects/0.1/projects/active";
    public override HttpMethod Method => HttpMethod.Get;
}