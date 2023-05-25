using Microsoft.Extensions.Localization;

namespace App.BuildingBlocks.Application.Emails.Templates;

public interface IEmailTemplateModel
{
    string TemplateName { get; }
    
    IStringLocalizer? Localizer { get; set; }
}
