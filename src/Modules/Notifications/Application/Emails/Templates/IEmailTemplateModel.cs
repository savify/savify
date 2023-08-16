using Microsoft.Extensions.Localization;

namespace App.Modules.Notifications.Application.Emails.Templates;

public interface IEmailTemplateModel
{
    string TemplateName { get; }

    IStringLocalizer? Localizer { get; set; }
}
