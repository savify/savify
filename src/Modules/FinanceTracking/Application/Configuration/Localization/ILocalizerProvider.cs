using Microsoft.Extensions.Localization;

namespace App.Modules.FinanceTracking.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
