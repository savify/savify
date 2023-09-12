using App.Integrations.SaltEdge.Client;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Integration;

public class IntegrationModule
{
    internal static void Configure(IServiceCollection services, bool isProduction)
    {
        services.AddScoped<ISaltEdgeIntegrationService>(provider => new SaltEdgeIntegrationService(
            provider.GetService<ISaltEdgeHttpClient>(),
            isProduction));
    }
}
