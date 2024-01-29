using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Currencies;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ExchangeRates;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

internal static class IntegrationServicesCollectionExtensions
{
    internal static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddScoped<ISaltEdgeIntegrationService, SaltEdgeIntegrationService>();
        services.AddScoped<ISaltEdgeCurrenciesProvider, SaltEdgeIntegrationService>();
        services.AddScoped<ISaltEdgeExchangeRatesProvider, SaltEdgeIntegrationService>();

        return services;
    }
}
