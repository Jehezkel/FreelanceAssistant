namespace WebApi.Models;

public class MailSettings
{
    public string Mail { get; set; } = "";
    public string Password { get; set; } = "";
    public SmtpSettings SMTP { get; set; } = new();
}
public class SmtpSettings
{
    public string Hostname { get; set; } = "";
    public bool SSL { get; set; }
    public bool TLS { get; set; }
    public int SSLPort { get; set; }
    public int TLSPort { get; set; }
}