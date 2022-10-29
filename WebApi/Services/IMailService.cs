using WebApi.Models;

namespace WebApi.Services;

public interface IMailService
{
    Task SendActivationMail(AppUser user, string code);
    Task SendEmailAsync(AppUser user, string subject, string body);
    Task SendResetConfirmation(AppUser user, string code);
}
