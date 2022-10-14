using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
namespace WebApi.DAL;

public class FLDbContext : IdentityDbContext<AppUser>
{
    public FLDbContext(DbContextOptions<FLDbContext> options) : base(options)
    {

    }

}