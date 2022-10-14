namespace WebApi.Models;
public class UserToken
{

    public string TokenValue { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public string UserID { get; set; } = null!;
    public TokenType Type { get; set; }

}
public enum TokenType
{
    Refresh,
    Activation,
    Reset
}
