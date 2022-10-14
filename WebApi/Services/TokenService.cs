using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;

namespace WebApi.Services;
public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
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
        roles.ToList().ForEach(r => claims.Add(new Claim("role", r)));
        var jwt = new JwtSecurityToken(
            issuer: jwtSection["ValidIssuer"],
            audience: jwtSection["ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSection["ExpireTime"])),
            claims: claims,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}