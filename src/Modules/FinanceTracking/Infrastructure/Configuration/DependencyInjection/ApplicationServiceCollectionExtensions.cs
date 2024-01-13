using App.Modules.FinanceTracking.Application.Currencies;
using App.Modules.FinanceTracking.Application.ExchangeRates;
using App.Modules.FinanceTracking.Infrastructure.Application.Currencies;
using App.Modules.FinanceTracking.Infrastructure.Application.ExchangeRates;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

internal static class ApplicationServiceCollectionExtensions
{
    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrenciesProvider, CurrenciesProvider>();
        services.AddScoped<IExchangeRatesProvider, ExchangeRatesProvider>();

        return services;
    }
}
