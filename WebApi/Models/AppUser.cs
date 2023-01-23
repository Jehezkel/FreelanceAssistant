using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;
public class AppUser : IdentityUser
{
    public IEnumerable<UserTempToken> UserTempTokens { get; set; } = new List<UserTempToken>();
    public IEnumerable<BidTemplate> DescriptionTemplates { get; set; } = new List<BidTemplate>();
    public virtual FLApiToken? FLApiToken { get; set; }
    public IEnumerable<ProjectSearch> projectSearches { get; set; } = new List<ProjectSearch>();
    public enum Roles
    {
        Admin,
        User
    }
}