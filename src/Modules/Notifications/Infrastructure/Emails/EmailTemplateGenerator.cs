using System.Text;
using App.BuildingBlocks.Application.Emails.Templates;
using RazorEngineCore;

namespace App.Modules.Notifications.Infrastructure.Emails;

public class EmailTemplateGenerator : IEmailTemplateGenerator
{
    public string GenerateEmailTemplate(IEmailTemplateModel model)
    {
        var emailTemplate = LoadEmailTemplate(model.TemplateName);

        IRazorEngine razorEngine = new RazorEngine();
        IRazorEngineCompiledTemplate compiledEmailTemplate = razorEngine.Compile(emailTemplate);

        return compiledEmailTemplate.Run(model);
    }
    
    private string LoadEmailTemplate(string templateName)
    {
        string notificationsResourcesPath = "Resources/Modules/Notifications";
        string relativePath = Path.Combine(notificationsResourcesPath, $"Templates/Emails/{templateName}.cshtml");
        string templatePath = Path.GetFullPath(relativePath);
        
        using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);

        string emailTemplate = streamReader.ReadToEnd();
        streamReader.Close();

        return emailTemplate;
    }
}
