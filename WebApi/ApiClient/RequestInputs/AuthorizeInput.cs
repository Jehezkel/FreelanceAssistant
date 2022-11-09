using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.RequestInputs;
public class AuthorizeInput
{
    public AuthorizeInput()
    {
    }
    public AuthorizeInput(FreelancerConfig freelancerConfig)
    {
        this.ResponseType = "code";
        this.ClientId = freelancerConfig.ClientID;
        this.RedirectUri = freelancerConfig.RedirectUri;
        this.Scope = "basic";
        this.AdvancedScopes = new List<int> { 2, 6 };
    }
    [UseInRequestParameters("response_type")]
    public string ResponseType { get; set; } = null!;
    [UseInRequestParameters("client_id")]
    public string ClientId { get; set; } = null!;
    [UseInRequestParameters("redirect_uri")]
    public string RedirectUri { get; set; } = null!;
    [UseInRequestParameters("scope")]
    public string Scope { get; set; } = null!;
    [UseInRequestParameters("advanced_scopes")]
    [FormatValue("{0}", " ")]
    public List<int> AdvancedScopes { get; set; } = null!;
}
