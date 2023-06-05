using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Emails;

public interface IEmailMessageFactory
{
    EmailMessage CreateLocalizedEmailMessage<T>(string receiverEmail, string subject, T templateModel, string culture)
        where T : IEmailTemplateModel;
}
