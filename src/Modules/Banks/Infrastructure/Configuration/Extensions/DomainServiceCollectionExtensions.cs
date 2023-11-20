using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Extensions;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IBanksSynchronisationService, BanksSynchronisationService>();

        return services;
    }
}
