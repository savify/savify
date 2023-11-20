using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.Wallets.Infrastructure.Configuration.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Modules.Wallets.Infrastructure.Configuration.Extensions;

internal static class LocalizationServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizer>(provider =>
        {
            var localizerFactory = provider.GetRequiredService<ILocalizerFactory>();

            return localizerFactory.Create<WalletsLocalizationResource>();
        });

        return services;
    }
}
