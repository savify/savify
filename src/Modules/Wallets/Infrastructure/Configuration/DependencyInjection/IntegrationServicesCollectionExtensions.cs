using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Wallets.Infrastructure.Configuration.DependencyInjection;

internal static class IntegrationServicesCollectionExtensions
{
    internal static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddScoped<ISaltEdgeIntegrationService, SaltEdgeIntegrationService>();

        return services;
    }
}
