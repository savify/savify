using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;

public class DebitWalletEditedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public Currency? NewCurrency { get; }

    public int? NewBalance { get; }

    public DebitWalletEditedDomainEvent(WalletId walletId, UserId userId, Currency? newCurrency, int? newBalance)
    {
        WalletId = walletId;
        UserId = userId;
        NewCurrency = newCurrency;
        NewBalance = newBalance;
    }
}
