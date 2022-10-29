using Microsoft.EntityFrameworkCore;
using WebApi.DAL;
using WebApi.Models;

namespace WebApi.Repositories;

public class FLApiTokenRepository : IFLApiTokenRepository
{
    private readonly IFLDbContext _fLDbContext;

    public FLApiTokenRepository(IFLDbContext fLDbContext)
    {
        _fLDbContext = fLDbContext;
    }

    public async Task<int> CreateFLApiToken(FLApiToken token)
    {
        _fLDbContext.FLApiTokens.Add(token);
        return await _fLDbContext.SaveChangesAsync();
    }

    public async Task<string> GetAccessToken(string UserId)
    {
        return await _fLDbContext.FLApiTokens.Where(t => t.UserID == UserId)
                                            .Select(t => t.AccessToken).FirstOrDefaultAsync() ?? "";
    }

    public async Task<string> GetRefreshToken(string UserId)
    {
        return await _fLDbContext.FLApiTokens.Where(t => t.UserID == UserId)
                                            .Select(t => t.RefreshToken).FirstOrDefaultAsync() ?? "";
    }

    public async Task<int> UpdateToken(string refreshToken, FLApiToken token)
    {
        var currToken = await _fLDbContext.FLApiTokens.Where(t => t.RefreshToken == refreshToken).FirstOrDefaultAsync();
        // currToken!.AccessToken = token.AccessToken;
        // currToken.RefreshToken = token.RefreshToken;
        // currToken.ExpireDate = token.ExpireDate;
        //not sure if this assignment will work...
        currToken = token;
        return await _fLDbContext.SaveChangesAsync();
    }
}