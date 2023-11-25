using App.Modules.Categories.Application.Configuration.Localization;
using App.Modules.Categories.Infrastructure.Configuration.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class LocalizationServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizerProvider, LocalizerProvider>();

        return services;
    }
}
