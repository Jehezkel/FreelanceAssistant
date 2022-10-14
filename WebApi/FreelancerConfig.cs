namespace WebApi;

public class FreelancerConfig
{
    public string ClientID { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string AuthEndpoint { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string BaseAddress { get; set; } = null!;
}