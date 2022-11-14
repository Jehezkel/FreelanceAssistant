using WebApi.Models;

namespace WebApi.Services
{
    public interface ITokenService
    {
        Task<string> CreateActivationTokenAsync(string userId);
        Task<string> CreateRefreshTokenAsync(string userId);
        Task<string> CreateResetTokenAsync(string userId);
        Task<string> CreateTokenAsync(string userId, TokenType tokenType);
        string GenerateAccessToken(AppUser user, IEnumerable<string> roles);
    }
}