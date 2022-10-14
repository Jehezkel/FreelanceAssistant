namespace WebApi.Account;
public class LoginCommand
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
public class LoginResult
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpireDate { get; set; }
    public string UserName { get; set; } = null!;
    public List<string> UserRoles { get; set; } = new List<string>();
}