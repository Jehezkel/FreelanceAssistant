using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class VerifyCodeRequest : BaseRequest<VerifyCodeInput>
    {
        public override string EndpointUrl => "token";

        public override HttpMethod Method => HttpMethod.Post;
    }
}
