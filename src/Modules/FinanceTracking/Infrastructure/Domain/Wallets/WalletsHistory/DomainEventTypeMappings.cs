using App.Modules.FinanceTracking.Domain.Wallets.Events;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;

internal static class DomainEventTypeMappings
{
    internal static IDictionary<string, Type> Dictionary { get; }

    static DomainEventTypeMappings()
    {
        Dictionary = new Dictionary<string, Type>();

        Dictionary.Add("WalletBalanceIncreased", typeof(WalletBalanceIncreasedDomainEvent));
        Dictionary.Add("WalletBalanceDecreased", typeof(WalletBalanceDecreasedDomainEvent));
    }
}
