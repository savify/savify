using App.Modules.Banks.Domain.BankSynchronisationProcessing.Services;
using App.Modules.Banks.Infrastructure.Domain.BankSynchronisationProcessing;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Domain;

internal static class DomainModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddScoped<IBankSynchronisationService, BankSynchronisationService>();
    }
}
