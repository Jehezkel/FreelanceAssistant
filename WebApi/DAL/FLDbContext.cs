using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DAL;

public class FLDbContext : IdentityDbContext<AppUser>, IFLDbContext
{
    public FLDbContext(DbContextOptions<FLDbContext> options) : base(options)
    {

    }
    public DbSet<UserTempToken> UserTempTokens => Set<UserTempToken>();

    public DbSet<FLApiToken> FLApiTokens => Set<FLApiToken>();

    public DbSet<BidTemplate> BidTemplates => Set<BidTemplate>();
    public DbSet<ProjectSearch> ProjectSearches => Set<ProjectSearch>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Seed();
        base.OnModelCreating(builder);

        builder.Entity<UserTempToken>()
        .HasOne(t => t.User)
        .WithMany(u => u.UserTempTokens)
        .HasForeignKey(t => t.UserID);
        builder.Entity<UserTempToken>().HasKey(t => new { t.UserID, t.Type });
        builder.Entity<UserTempToken>()
        .Property(t => t.Type).HasConversion(v => v.ToString(), v => (TokenType)Enum.Parse(typeof(TokenType), v));

        builder.Entity<FLApiToken>().HasKey(t => t.UserId);
        builder.Entity<FLApiToken>()
        .HasOne(t => t.User)
        .WithOne(u => u.FLApiToken)
        .HasForeignKey<FLApiToken>(t => t.UserId);

        builder.Entity<BidTemplate>().HasKey(t => t.Id);
        builder.Entity<BidTemplate>()
        .HasOne(d => d.User)
        .WithMany(u => u.DescriptionTemplates)
        .HasForeignKey(t => t.UserId);

        builder.Entity<ProjectSearch>().HasKey(p => p.Id);
        builder.Entity<ProjectSearch>().HasOne(p => p.User)
            .WithMany(u => u.projectSearches).HasForeignKey(p => p.UserId);
        builder.Entity<ProjectSearch>()
            .Property(p => p.Input)
            .HasColumnType("jsonb");

    }



}