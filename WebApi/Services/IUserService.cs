using WebApi.Account;
using WebApi.Models;

namespace WebApi.Services
{
    public interface ICurrentUserService
    {
        public string? UserId { get;}
        public bool IsAdmin { get; }
        //public Task<AppUser> GetCurrentUser();
    }
}
