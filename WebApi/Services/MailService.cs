using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApi.Models;

namespace WebApi.Services;

public class MailService
{
    private readonly MailSettings _settings;
    private readonly ILogger<MailService> _logger;
    private readonly SmtpClient _client;

    public MailService(IOptions<MailSettings> options, ILogger<MailService> logger)
    {
        _logger = logger;
        _settings = options.Value;
        _client = new SmtpClient();

    }
    public async Task SendEmailAsync(AppUser user, string subject, string body)
    {
        await _client.ConnectAsync(_settings.SMTP.Hostname, _settings.SMTP.SSLPort, SecureSocketOptions.SslOnConnect);
        await _client.AuthenticateAsync(_settings.Mail, _settings.Password);
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress("Freelance Assistant", _settings.Mail));
        msg.To.Add(new MailboxAddress(user.UserName, user.Email));
        msg.Subject = $"Freelance Notification | {subject}";
        msg.Body = (new BodyBuilder
        {
            TextBody = body
        }).ToMessageBody();
        await _client.SendAsync(msg);
        await _client.DisconnectAsync(true);
    }
    public async Task SendActivationMail(AppUser user, string code)
    {
        string body = $"Hello {user.UserName}! \n Your activation code is {code}";
        await SendEmailAsync(user, "Account Activation", body);
    }
}