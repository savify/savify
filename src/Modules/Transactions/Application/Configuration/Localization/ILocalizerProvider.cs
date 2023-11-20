using Microsoft.Extensions.Localization;

namespace App.Modules.Transactions.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
