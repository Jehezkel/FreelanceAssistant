using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;
public class AppUser : IdentityUser
{
    public IEnumerable<UserTempToken> UserTempTokens { get; set; } = new List<UserTempToken>();
    public virtual FLApiToken? FLApiToken { get; set; } 
}