using System.Text;
using App.Modules.Notifications.Application.Emails.Templates;
using RazorEngineCore;

namespace App.Modules.Notifications.Infrastructure.Emails;

public class EmailTemplateGenerator : IEmailTemplateGenerator
{
    public string GenerateEmailTemplate(IEmailTemplateModel model)
    {
        var emailTemplate = LoadEmailTemplate(model.TemplateName);

        IRazorEngine razorEngine = new RazorEngine();
        var compiledEmailTemplate = razorEngine.Compile(emailTemplate);

        return compiledEmailTemplate.Run(model);
    }

    private string LoadEmailTemplate(string templateName)
    {
        var notificationsResourcesPath = "Resources/Modules/Notifications";
        var relativePath = Path.Combine(notificationsResourcesPath, $"Templates/Emails/{templateName}.cshtml");
        var templatePath = Path.GetFullPath(relativePath);

        using var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, Encoding.Default);

        var emailTemplate = streamReader.ReadToEnd();
        streamReader.Close();

        return emailTemplate;
    }
}
