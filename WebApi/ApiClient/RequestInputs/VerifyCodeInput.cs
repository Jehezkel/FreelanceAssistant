using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.RequestInputs
{
    public class VerifyCodeInput
    {
        public VerifyCodeInput()
        {
        }
        public VerifyCodeInput(FreelancerConfig config, string code)
        {
            this.RedirectUri = config.RedirectUri;
            this.ClientSecret = config.ClientSecret;
            this.ClientId= config.ClientID;
            this.Code = code;
        }
        [UseInUrlEncodedBody("grant_type")]
        public string GrantType { get; set; } = "authorization_code";
        [UseInUrlEncodedBody("code")]
        public string Code { get; set; } = "";
        [UseInUrlEncodedBody("client_id")]
        public string ClientId { get; set; } = "";
        [UseInUrlEncodedBody("client_secret")]
        public string ClientSecret { get; set; } = "";
        [UseInUrlEncodedBody("redirect_uri")]
        public string RedirectUri { get; set; } = "";
    }
}
