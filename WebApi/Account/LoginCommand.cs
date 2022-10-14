namespace WebApi.Account;
public class LoginCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class LoginResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }
    public string UserName { get; set; }
}