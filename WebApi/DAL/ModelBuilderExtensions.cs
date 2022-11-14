using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DAL;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        List<IdentityRole> identityRoles = new List<IdentityRole>();
        Enum.GetNames(typeof(AppUser.Roles)).ToList().ForEach(r =>
        {
            identityRoles.Add(new IdentityRole { Name = r, NormalizedName = r.ToUpper() });
        });
        modelBuilder.Entity<IdentityRole>().HasData(identityRoles);
    }
}
