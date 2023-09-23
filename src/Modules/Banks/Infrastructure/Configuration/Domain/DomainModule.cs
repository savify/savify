using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Domain;

internal static class DomainModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddScoped<IBanksSynchronisationService, BanksSynchronisationService>();
    }
}
