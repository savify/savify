using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.DependencyInjection;

internal static class IntegrationServicesCollectionExtensions
{
    internal static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        // services.AddScoped<ISaltEdgeIntegrationService>(provider => new SaltEdgeIntegrationService(
        //     provider.GetService<ISaltEdgeHttpClient>()));

        return services;
    }
}
