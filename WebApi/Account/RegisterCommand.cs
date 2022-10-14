namespace WebApi.Account;
public class RegisterCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
}
public class RegisterResult
{
    public string Email { get; set; }
}