using App.Integrations.SaltEdge.Client;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class IntegrationServicesCollectionExtensions
{
    internal static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        // services.AddScoped<ISaltEdgeIntegrationService>(provider => new SaltEdgeIntegrationService(
        //     provider.GetService<ISaltEdgeHttpClient>()));

        return services;
    }
}
