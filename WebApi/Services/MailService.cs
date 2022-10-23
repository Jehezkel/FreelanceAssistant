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
    private readonly MailTemplateService _templateService;
    private readonly SmtpClient _client;

    public MailService(IOptions<MailSettings> options, ILogger<MailService> logger, MailTemplateService templateService)
    {
        _logger = logger;
        _templateService = templateService;
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
        msg.Subject = $"FL Assistant | {subject}";
        msg.Body = (new BodyBuilder
        {
            HtmlBody = body
        }).ToMessageBody();
        await _client.SendAsync(msg);
        await _client.DisconnectAsync(true);
    }
    public async Task SendActivationMail(AppUser user, string code)
    {
        string htmlContent = $@"             <td>
                        <p>Hello {user.UserName}!</p>
                        <p>Welcome to our site. To activate account please use below link.</p>
                        <table role='presentation' border='0' cellpadding='0' cellspacing='0' class='btn btn-primary'>
                          <tbody>
                            <tr>
                              <td align='center'>
                                <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
                                  <tbody>
                                    <tr>
                                      <td> <a href='{_settings.RedirectBase}/activate?tokenValue={code}' target='_blank'>Activate</a> </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                        <p> If you did not intend to create account on Freelancer assistant, please ignore that mail.</p>
                        <p> Regards,</p>
                        <p> FL Assistant Admin</p>
                      </td>";
        string htmlBody = _templateService.PrepareMailBody(htmlContent);
        await SendEmailAsync(user, "Account Activation", htmlBody);
    }
    public async Task SendResetConfirmation(AppUser user, string code)
    {
        string body = $"Hello {user.UserName}! \n Your activation code is {code}";
        await SendEmailAsync(user, "Password Reset Request", body);
    }
}