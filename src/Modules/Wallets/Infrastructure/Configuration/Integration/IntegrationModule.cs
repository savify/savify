using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Wallets.Infrastructure.Configuration.Integration;

public class IntegrationModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddScoped<ISaltEdgeIntegrationService, SaltEdgeIntegrationService>();
    }
}
