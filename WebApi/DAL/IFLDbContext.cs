using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DAL;

public interface IFLDbContext
{
    DbSet<UserTempToken> UserTempTokens { get; }
    DbSet<FLApiToken> FLApiTokens { get; }
    DbSet<BidTemplate> BidTemplates { get; }
    DbSet<ProjectSearch> ProjectSearches { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
