using Microsoft.Extensions.Localization;

namespace App.Modules.Wallets.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
