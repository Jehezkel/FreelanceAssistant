using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DAL;

public class FLDbContext : IdentityDbContext<AppUser>
{
    public FLDbContext(DbContextOptions<FLDbContext> options) : base(options)
    {

    }
    public DbSet<UserTempToken> UserTempTokens => Set<UserTempToken>();
    public DbSet<FLApiToken> FLApiTokens => Set<FLApiToken>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserTempToken>()
        .HasOne(t => t.User)
        .WithMany(u => u.UserTempTokens)
        .HasForeignKey(t => t.UserID);

        builder.Entity<UserTempToken>().HasKey(t => new { t.UserID, t.Type });

        builder.Entity<UserTempToken>()
        .Property(t => t.Type).HasConversion(v => v.ToString(), v => (TokenType)Enum.Parse(typeof(TokenType), v));

        builder.Entity<FLApiToken>()
        .HasOne(t => t.User);
        builder.Entity<FLApiToken>().HasKey(t => t.UserID);
    }
}