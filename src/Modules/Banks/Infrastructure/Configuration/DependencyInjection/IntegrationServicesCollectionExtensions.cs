using App.Integrations.SaltEdge.Client;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.DependencyInjection;

internal static class IntegrationServicesCollectionExtensions
{
    internal static IServiceCollection AddIntegrationServices(this IServiceCollection services, bool isProduction)
    {
        services.AddScoped<ISaltEdgeIntegrationService>(provider => new SaltEdgeIntegrationService(
            provider.GetService<ISaltEdgeHttpClient>(),
            isProduction));

        return services;
    }
}
