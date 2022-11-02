using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.DAL;
using WebApi.Models;

namespace WebApi.Repositories;

public class FLApiTokenRepository : IFLApiTokenRepository
{
    private readonly IFLDbContext _fLDbContext;

    public FLApiTokenRepository(IFLDbContext fLDbContext, UserManager<AppUser> userManager)
    {
        _fLDbContext = fLDbContext;
    }

    public async Task<int> CreateFLApiToken(FLApiToken token)
    {
        //var user = await _userManager.FindByIdAsync(token.UserId);
        //token.User = user;
        _fLDbContext.FLApiTokens.Add(token);
        return await _fLDbContext.SaveChangesAsync();
    }
    public async Task<FLApiToken?> GetFLApiToken(string UserId)
    {
        return await _fLDbContext.FLApiTokens.Where(t => t.UserId == UserId)
                                           .FirstOrDefaultAsync();
    }
    public async Task<string?> GetAccessToken(string UserId)
    {
        return await _fLDbContext.FLApiTokens.Where(t => t.UserId == UserId)
                                            .Select(t => t.AccessToken).FirstOrDefaultAsync();
    }

    public async Task<string?> GetRefreshToken(string UserId)
    {
        return await _fLDbContext.FLApiTokens.Where(t => t.UserId == UserId)
                                            .Select(t => t.RefreshToken).FirstOrDefaultAsync();
    }

    public async Task<int> UpdateToken(string refreshToken, FLApiToken token)
    {
        var currToken = await _fLDbContext.FLApiTokens.Where(t => t.RefreshToken == refreshToken).FirstOrDefaultAsync();
        currToken = token;
        return await _fLDbContext.SaveChangesAsync();
    }
}