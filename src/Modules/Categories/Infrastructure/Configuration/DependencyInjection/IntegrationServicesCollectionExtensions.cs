using App.Integrations.SaltEdge.Client;
using App.Modules.Categories.Infrastructure.Integrations.SaltEdge;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class IntegrationServicesCollectionExtensions
{
    internal static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddScoped<ISaltEdgeIntegrationService>(provider => new SaltEdgeIntegrationService(
            provider.GetRequiredService<ISaltEdgeHttpClient>()));

        return services;
    }
}
