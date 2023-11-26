using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
