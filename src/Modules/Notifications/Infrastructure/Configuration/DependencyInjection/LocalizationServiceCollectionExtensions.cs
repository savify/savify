using App.Modules.Notifications.Application.Configuration.Localization;
using App.Modules.Notifications.Infrastructure.Configuration.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.Infrastructure.Configuration.DependencyInjection;

internal static class LocalizationServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizerProvider, LocalizerProvider>();

        return services;
    }
}
