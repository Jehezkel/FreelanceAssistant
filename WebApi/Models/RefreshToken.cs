namespace WebApi.Models;
public class UserTempToken
{

    public string TokenValue { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public string UserID { get; set; } = null!;
    public TokenType Type { get; set; }
    public DateTimeOffset CreateDate { get; set; }

}
public enum TokenType
{
    Refresh,
    Activation,
    Reset
}
