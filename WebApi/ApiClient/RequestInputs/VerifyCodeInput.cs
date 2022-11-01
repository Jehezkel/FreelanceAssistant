using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.RequestInputs
{
    public class VerifyCodeInput
    {
        public VerifyCodeInput()
        {
        }
        public VerifyCodeInput(FreelancerConfig config)
        {
            this.RedirectUri = config.RedirectUri;
            this.ClientSecret = config.ClientSecret;
            this.ClientId= config.ClientID;
        }
        //verifyCodeParams.Add("grant_type", "authorization_code");
        //verifyCodeParams.Add("code", code);
        //verifyCodeParams.Add("client_id", _freelancerConfig.ClientID);
        //verifyCodeParams.Add("client_secret", _freelancerConfig.ClientSecret);
        //verifyCodeParams.Add("redirect_uri", _freelancerConfig.RedirectUri);
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
