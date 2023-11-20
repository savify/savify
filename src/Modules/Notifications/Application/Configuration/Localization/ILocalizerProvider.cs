using Microsoft.Extensions.Localization;

namespace App.Modules.Notifications.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
