using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Events;

public class CashWalletAddedDomainEvent(WalletId walletId, UserId userId, Currency currency) : DomainEventBase
{
    public WalletId WalletId { get; } = walletId;

    public UserId UserId { get; } = userId;

    public Currency Currency { get; } = currency;
}
