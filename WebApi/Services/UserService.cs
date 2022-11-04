using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using WebApi.Account;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<LoginResult> Create()
        {
            throw new NotImplementedException();

        }

        public Task<UserTempToken> CreateActivationToken()
        {
            throw new NotImplementedException();
        }

        public Task<LoginResult> CreateLoginResult(AppUser user)
        {
            //var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            //var refreshToken = await GenerateTempToken(user, TokenType.Refresh);
            //var result = new LoginResult
            //{
            //    UserName = user.UserName,
            //    RefreshToken = refreshToken.TokenValue,
            //    //AccessToken = _tokenService.GenerateAccessToken(user, userRoles),
            //    UserRoles = userRoles
            //};
            throw new NotImplementedException();
        }

        public Task<UserTempToken> CreateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCurrentUserId()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetCurrentUserRoles()
        {
            throw new NotImplementedException();
        }
    }
}
