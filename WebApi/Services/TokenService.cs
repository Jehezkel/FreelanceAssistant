using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Duende.IdentityServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.DAL;
using WebApi.Migrations;
using WebApi.Models;

namespace WebApi.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly FLDbContext _fLDbContext;

    public TokenService(IConfiguration config, FLDbContext fLDbContext)
    {
        _config = config;
        _fLDbContext = fLDbContext;
    }
    public string GenerateAccessToken(AppUser user, IEnumerable<string> roles)
    {
        var jwtSection = _config.GetSection("Authentication:Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new Claim("name", user.UserName));
        claims.Add(new Claim("mail", user.Email));
        roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
        var jwt = new JwtSecurityToken(
            issuer: jwtSection["ValidIssuer"],
            audience: jwtSection["ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSection["ExpireTime"])),
            claims: claims,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    public async Task<string> CreateRefreshTokenAsync(string userId) =>
                                            await CreateTokenAsync(userId, TokenType.Refresh);
    public async Task<string> CreateActivationTokenAsync(string userId) =>
                                            await CreateTokenAsync(userId, TokenType.Activation);
    public async Task<string> CreateResetTokenAsync(string userId) =>
                                            await CreateTokenAsync(userId, TokenType.Reset);
    public async Task<string> CreateTokenAsync(string userId, TokenType tokenType)
    {
        var newUserTempToken = CreateUserTempToken(userId, tokenType);
        await SaveTokenAsync(newUserTempToken);
        return newUserTempToken.TokenValue;
    }

    private UserTempToken CreateUserTempToken(string userId, TokenType tokenType)
    {
        var tokenVal = tokenType == TokenType.Refresh ? GenerateRefreshTokenValue() : Guid.NewGuid().ToString();
        var token = new UserTempToken
        {
            UserID = userId,
            TokenValue = tokenVal,
            Type = tokenType
        };
        return token;
    }

    private string GenerateRefreshTokenValue()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    private async Task SaveTokenAsync(UserTempToken newToken)
    {
        var currentToken = await _fLDbContext.UserTempTokens
            .Where(t => t.UserID == newToken.UserID && t.Type == newToken.Type).FirstOrDefaultAsync();
        if (currentToken is null)
        {
            currentToken = newToken;
            await _fLDbContext.UserTempTokens.AddAsync(currentToken);
        }
        else
        {
            currentToken.TokenValue = newToken.TokenValue;
        }
        currentToken.CreateDate = DateTimeOffset.UtcNow;
        await _fLDbContext.SaveChangesAsync();

    }
}