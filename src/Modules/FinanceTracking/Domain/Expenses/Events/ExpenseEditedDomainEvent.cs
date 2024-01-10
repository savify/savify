using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Expenses.Events;

public class ExpenseEditedDomainEvent(
    UserId userId,
    WalletId oldSourceWalletId,
    WalletId newSourceWalletId,
    Money oldAmount,
    Money newAmount,
    IEnumerable<string> tags) : DomainEventBase
{
    public UserId UserId { get; } = userId;

    public WalletId OldSourceWalletId { get; } = oldSourceWalletId;
    public WalletId NewSourceWalletId { get; } = newSourceWalletId;

    public Money OldAmount { get; } = oldAmount;
    public Money NewAmount { get; } = newAmount;

    public IEnumerable<string> Tags { get; } = tags;
}
