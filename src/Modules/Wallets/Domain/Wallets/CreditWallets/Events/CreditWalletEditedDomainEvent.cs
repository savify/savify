using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CreditWallets.Events;

public class CreditWalletEditedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public Currency? NewCurrency { get; }

    public int? NewAvailableBalance { get; }

    public int? NewCreditLimit { get; }

    public CreditWalletEditedDomainEvent(WalletId walletId, UserId userId, Currency? newCurrency, int? newAvailableBalance, int? newCreditLimit)
    {
        WalletId = walletId;
        UserId = userId;
        NewCurrency = newCurrency;
        NewAvailableBalance = newAvailableBalance;
        NewCreditLimit = newCreditLimit;
    }
}
