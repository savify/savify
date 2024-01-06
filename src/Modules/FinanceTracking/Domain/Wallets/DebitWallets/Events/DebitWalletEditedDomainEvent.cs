using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;

public class DebitWalletEditedDomainEvent(WalletId walletId, UserId userId, int? newBalance) : DomainEventBase
{
    public WalletId WalletId { get; } = walletId;

    public UserId UserId { get; } = userId;

    public int? NewBalance { get; } = newBalance;
}
