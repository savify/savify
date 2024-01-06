using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Incomes.Events;

public class IncomeEditedDomainEvent(
    UserId userId,
    WalletId oldTargetWalletId,
    WalletId newTargetWalletId,
    Money oldAmount,
    Money newAmount,
    IEnumerable<string> tags) : DomainEventBase
{
    public UserId UserId { get; } = userId;

    public WalletId OldTargetWalletId { get; } = oldTargetWalletId;
    public WalletId NewTargetWalletId { get; } = newTargetWalletId;

    public Money OldAmount { get; } = oldAmount;
    public Money NewAmount { get; } = newAmount;

    public IEnumerable<string> Tags { get; } = tags;
}
