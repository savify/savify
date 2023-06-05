namespace App.Modules.Notifications.Application.Emails.Templates;

public interface IEmailTemplateGenerator
{
    string GenerateEmailTemplate(IEmailTemplateModel model);
}
