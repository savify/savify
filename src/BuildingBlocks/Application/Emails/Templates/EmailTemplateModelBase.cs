using Microsoft.Extensions.Localization;

namespace App.BuildingBlocks.Application.Emails.Templates;

public abstract class EmailTemplateModelBase : IEmailTemplateModel
{
    public abstract string TemplateName { get; }

    public IStringLocalizer? Localizer { get; set; }
}
