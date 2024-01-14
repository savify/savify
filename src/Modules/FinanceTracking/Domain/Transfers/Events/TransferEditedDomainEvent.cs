using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;
public class TransferEditedDomainEvent(
    UserId userId,
    WalletId oldSourceWalletId,
    WalletId newSourceWalletId,
    WalletId oldTargetWalletId,
    WalletId newTargetWalletId,
    TransactionAmount oldAmount,
    TransactionAmount newAmount,
    IEnumerable<string> tags) : DomainEventBase
{
    public UserId UserId { get; } = userId;

    public WalletId OldSourceWalletId { get; } = oldSourceWalletId;
    public WalletId NewSourceWalletId { get; } = newSourceWalletId;

    public WalletId OldTargetWalletId { get; } = oldTargetWalletId;
    public WalletId NewTargetWalletId { get; } = newTargetWalletId;

    public TransactionAmount OldAmount { get; } = oldAmount;
    public TransactionAmount NewAmount { get; } = newAmount;

    public IEnumerable<string> Tags { get; } = tags;
}
