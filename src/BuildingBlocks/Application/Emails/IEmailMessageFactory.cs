using App.BuildingBlocks.Application.Emails.Templates;

namespace App.BuildingBlocks.Application.Emails;

public interface IEmailMessageFactory
{
    EmailMessage CreateLocalizedEmailMessage<T>(string receiverEmail, string subject, T templateModel, string culture)
        where T : IEmailTemplateModel;
}
