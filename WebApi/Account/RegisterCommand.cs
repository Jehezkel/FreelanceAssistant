namespace WebApi.Account;
public class RegisterCommand
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
public class RegisterResult
{
    public string Email { get; set; } = null!;
}