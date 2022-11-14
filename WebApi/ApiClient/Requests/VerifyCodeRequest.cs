using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class VerifyCodeRequest : BaseRequest<VerifyCodeInput>
    {
        public VerifyCodeRequest()
        {

        }
        public VerifyCodeRequest(FreelancerConfig freelancerConfig, string code)
        {
            this.RequestInputObject = new VerifyCodeInput(freelancerConfig,code);
        }
        public override string EndpointUrl => "token";

        public override HttpMethod Method => HttpMethod.Post;
    }
}
