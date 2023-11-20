using App.Modules.UserAccess.Application.Configuration.Localization;
using App.Modules.UserAccess.Infrastructure.Configuration.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Extensions;

internal static class LocalizationServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizerProvider, LocalizerProvider>();

        return services;
    }
}
