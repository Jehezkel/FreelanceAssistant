using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class AddJobsRequest : BaseRequest<AddJobsInput>
    {
        public override string EndpointUrl => "users/0.1/self/jobs";

        public override HttpMethod Method => HttpMethod.Post;
    }
}
