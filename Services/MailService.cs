using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using webapi.Models;

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
    public async Task SendEmailAsync()
    {
        await _client.ConnectAsync(_settings.SMTP.Hostname, _settings.SMTP.SSLPort, SecureSocketOptions.SslOnConnect);
        await _client.AuthenticateAsync(_settings.Mail, _settings.Password);
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress("Freelance Assistant", _settings.Mail));
        msg.To.Add(new MailboxAddress("Your mama", "heylookitscorpo@gmail.com"));
        msg.Subject = $"{DateTime.Now} Wake up";
        await _client.SendAsync(msg);
        await _client.DisconnectAsync(true);
    }
}