using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DAL;

public class FLDbContext : IdentityDbContext<AppUser>
{
    public FLDbContext(DbContextOptions<FLDbContext> options) : base(options)
    {

    }
    public DbSet<UserToken> UserTokens { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserToken>()
        .HasOne(t => t.User)
        .WithMany(u => u.UserTokens)
        .HasForeignKey(t => t.UserID);

        builder.Entity<UserToken>().HasKey(t => new { t.UserID, t.Type });

        builder.Entity<UserToken>()
        .Property(t => t.Type).HasConversion(v => v.ToString(), v => (TokenType)Enum.Parse(typeof(TokenType), v));
    }
}