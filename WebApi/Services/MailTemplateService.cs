using Microsoft.Extensions.Options;

namespace WebApi.Services;

public class MailTemplateService
{
    private string mailTemplate;
    public MailTemplateService()
    {
        this.mailTemplate = System.IO.File.ReadAllText("MailTemplates/Main.html");
    }

    public string PrepareMailBody(string content)
    {
        return this.mailTemplate.Replace("{CONTENT}", content);
    }
}