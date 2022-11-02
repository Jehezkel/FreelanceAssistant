using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests;
public class CreateBidRequest : BaseRequest<CreateBidInput>
{
    public override string EndpointUrl => "projects/0.1/bids";

    public override HttpMethod Method => HttpMethod.Post;
}
