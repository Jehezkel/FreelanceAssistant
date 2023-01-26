using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string? UserId => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsAdmin => _httpContextAccessor.HttpContext!.User.IsInRole(AppUser.Roles.Admin.ToString());

    //public Task<AppUser> GetCurrentUser() => _httpContextAccessor.HttpContext
}
