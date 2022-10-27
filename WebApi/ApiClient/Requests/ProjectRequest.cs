using WebApi.FreelanceQueries;

namespace WebApi.ApiClient.Requests;
public class ProjectRequest : BaseRequest<ProjectsFilter>
{
    public override string EndpointUrl => "/projects/0.1/projects/";

    public override HttpMethod Method => HttpMethod.Post;

    // public override ProjectsFilter SearchObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    // public void MaxPrice()
}