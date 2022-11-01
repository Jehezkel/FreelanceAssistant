namespace WebApi.Models;
public class FLApiToken
{
    public virtual AppUser User { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTimeOffset ExpireDate { get; set; }
    public override string ToString()
    {
        return $@"AccessToken: {this.AccessToken}
        RefreshToken: {this.RefreshToken}
        ExpireDate: {this.ExpireDate}
        ";
    }
}