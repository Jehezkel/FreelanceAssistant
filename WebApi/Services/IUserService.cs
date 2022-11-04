using WebApi.Account;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        public Task<int> GetCurrentUserId();
        public Task<AppUser> GetCurrentUser();
        public Task<List<string>> GetCurrentUserRoles();
    }
}
