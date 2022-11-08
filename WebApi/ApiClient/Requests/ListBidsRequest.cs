using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class ListBidsRequest : BaseRequest
    {
        public override string EndpointUrl => "projects/0.1/bids";

        public override HttpMethod Method => HttpMethod.Get;
    }
}
