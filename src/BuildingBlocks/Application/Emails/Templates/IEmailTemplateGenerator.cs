namespace App.BuildingBlocks.Application.Emails.Templates;

public interface IEmailTemplateGenerator
{
    string GenerateEmailTemplate(IEmailTemplateModel model);
}
