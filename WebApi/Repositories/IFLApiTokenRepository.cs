using WebApi.Models;

namespace WebApi.Repositories;

public interface IFLApiTokenRepository
{
    public Task<string> GetAccessToken(string UserId);
    public Task<string> GetRefreshToken(string UserId);
    public Task<int> CreateFLApiToken(FLApiToken token);
    public Task<int> UpdateToken(string refreshToken, FLApiToken token);
}