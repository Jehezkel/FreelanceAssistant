using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;
public class AppUser : IdentityUser
{
    public IEnumerable<UserTempToken> UserTempTokens { get; set; } = new List<UserTempToken>();
    public IEnumerable<DescriptionTemplate> DescriptionTemplates { get; set; } = new List<DescriptionTemplate>();
    public virtual FLApiToken? FLApiToken { get; set; } 
}