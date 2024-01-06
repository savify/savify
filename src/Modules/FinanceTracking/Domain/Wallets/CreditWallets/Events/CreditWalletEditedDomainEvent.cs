using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Events;

public class CreditWalletEditedDomainEvent(
    WalletId walletId,
    UserId userId,
    int? newAvailableBalance,
    int? newCreditLimit)
    : DomainEventBase
{
    public WalletId WalletId { get; } = walletId;

    public UserId UserId { get; } = userId;

    public int? NewAvailableBalance { get; } = newAvailableBalance;

    public int? NewCreditLimit { get; } = newCreditLimit;
}
