using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<CashWalletFactory>();
        services.AddScoped<CreditWalletFactory>();
        services.AddScoped<DebitWalletFactory>();
        services.AddScoped<CashWalletEditionService>();
        services.AddScoped<CreditWalletEditionService>();
        services.AddScoped<DebitWalletEditionService>();
        services.AddScoped<UserTagsUpdateService>();
        services.AddScoped<IBankConnectionProcessInitiationService, BankConnectionProcessInitiationService>();
        services.AddScoped<IBankConnectionProcessRedirectionService, BankConnectionProcessRedirectionService>();
        services.AddScoped<IBankConnectionProcessConnectionCreationService, BankConnectionProcessConnectionCreationService>();
        services.AddScoped<IBankAccountConnector, BankAccountConnector>();

        return services;
    }
}
