using App.Integrations.SaltEdge.Client;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.Integration;

public class IntegrationModule
{
    internal static void Configure(IServiceCollection services)
    {
        // services.AddScoped<ISaltEdgeIntegrationService>(provider => new SaltEdgeIntegrationService(
        //     provider.GetService<ISaltEdgeHttpClient>()));
    }
}
