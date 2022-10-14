using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;
public class AppUser : IdentityUser
{
    public IEnumerable<UserToken> UserTokens { get; set; } = new List<UserToken>();
}