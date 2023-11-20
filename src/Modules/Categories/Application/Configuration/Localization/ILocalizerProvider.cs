using Microsoft.Extensions.Localization;

namespace App.Modules.Categories.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
