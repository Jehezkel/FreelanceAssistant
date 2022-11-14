namespace WebApi.ApiClient.Requests
{
    public class SelfInformationRequest : BaseRequest
    {
        public override string EndpointUrl => "users/0.1/self";

        public override HttpMethod Method => HttpMethod.Get;
    }
}
