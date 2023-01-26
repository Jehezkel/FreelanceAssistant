namespace WebApi.Dtos.Account;
public class RefreshResult
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}