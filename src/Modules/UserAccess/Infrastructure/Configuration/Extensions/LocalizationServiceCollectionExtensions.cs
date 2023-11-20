using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.UserAccess.Infrastructure.Configuration.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Extensions;

internal static class LocalizationServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizer>(provider =>
        {
            var localizerFactory = provider.GetRequiredService<ILocalizerFactory>();

            return localizerFactory.Create<UserAccessLocalizationResource>();
        });

        return services;
    }
}
