using Microsoft.Extensions.Localization;

namespace App.Modules.Banks.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
