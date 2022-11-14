using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class JobsRequest : BaseRequest<JobsInput>
    {
        public override string EndpointUrl => "projects/0.1/jobs";

        public override HttpMethod Method => HttpMethod.Get;
    }
}
