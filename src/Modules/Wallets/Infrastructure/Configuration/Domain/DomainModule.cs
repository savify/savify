using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;
using App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Wallets.Infrastructure.Configuration.Domain;

internal static class DomainModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddScoped<IBankConnectionProcessInitiationService, BankConnectionProcessInitiationService>();
        services.AddScoped<IBankConnectionProcessRedirectionService, BankConnectionProcessRedirectionService>();
        services.AddScoped<IBankConnectionProcessConnectionCreationService, BankConnectionProcessConnectionCreationService>();
        services.AddScoped<IBankAccountConnector, BankAccountConnector>();
    }
}
