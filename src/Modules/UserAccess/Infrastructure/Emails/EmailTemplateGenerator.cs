using System.Text;
using App.BuildingBlocks.Application.Emails.Templates;
using RazorEngineCore;

namespace App.Modules.UserAccess.Infrastructure.Emails;

// TODO: create base abstract class and move there all except template path logic
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
        string userAccessResourcesPath = "Resources/Modules/UserAccess";
        string relativePath = Path.Combine(userAccessResourcesPath, $"Templates/Emails/{templateName}.cshtml");
        string templatePath = Path.GetFullPath(relativePath);
        
        using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);

        string emailTemplate = streamReader.ReadToEnd();
        streamReader.Close();

        return emailTemplate;
    }
}
