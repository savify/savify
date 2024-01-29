using App.Modules.FinanceTracking.Application.Currencies;
using App.Modules.FinanceTracking.Infrastructure.Application.Currencies;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

internal static class ApplicationServiceCollectionExtensions
{
    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrenciesProvider, CurrenciesProvider>();

        return services;
    }
}
