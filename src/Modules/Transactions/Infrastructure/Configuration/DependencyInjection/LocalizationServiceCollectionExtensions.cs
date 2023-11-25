using App.Modules.Transactions.Application.Configuration.Localization;
using App.Modules.Transactions.Infrastructure.Configuration.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.DependencyInjection;

internal static class LocalizationServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizerProvider, LocalizerProvider>();

        return services;
    }
}
