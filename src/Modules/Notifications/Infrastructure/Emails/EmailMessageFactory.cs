using System.Globalization;
using App.Modules.Notifications.Application.Configuration.Localization;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Emails.Templates;
using Microsoft.Extensions.Localization;

namespace App.Modules.Notifications.Infrastructure.Emails;

public class EmailMessageFactory : IEmailMessageFactory
{
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;

    private readonly IStringLocalizer _localizer;

    public EmailMessageFactory(IEmailTemplateGenerator emailTemplateGenerator, ILocalizerProvider localizerProvider)
    {
        _emailTemplateGenerator = emailTemplateGenerator;
        _localizer = localizerProvider.GetLocalizer();
    }

    public EmailMessage CreateLocalizedEmailMessage<T>(string receiverEmail, string subject, T templateModel, string culture) where T : IEmailTemplateModel
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(culture);

        templateModel.Localizer = _localizer;

        return new EmailMessage(
            receiverEmail,
            $"Savify - {templateModel.Localizer[subject]}",
            _emailTemplateGenerator.GenerateEmailTemplate(templateModel));
    }
}
