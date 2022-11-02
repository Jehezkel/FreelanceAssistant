using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests;
public class AuthorizeRequest : BaseRequest<AuthorizeInput>
{
    public AuthorizeRequest(FreelancerConfig freelancerConfig)
    {
        this.RequestInputObject = new AuthorizeInput(freelancerConfig);
    }
    public override string EndpointUrl => "authorize";

    public override HttpMethod Method =>HttpMethod.Get;
    
}
