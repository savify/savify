using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;

public class TransferEditedDomainEvent(
    UserId userId,
    WalletId oldSourceWalletId,
    WalletId newSourceWalletId,
    WalletId oldTargetWalletId,
    WalletId newTargetWalletId,
    TransferAmount oldAmount,
    TransferAmount newAmount,
    IEnumerable<string> tags) : DomainEventBase
{
    public UserId UserId { get; } = userId;

    public WalletId OldSourceWalletId { get; } = oldSourceWalletId;
    public WalletId NewSourceWalletId { get; } = newSourceWalletId;

    public WalletId OldTargetWalletId { get; } = oldTargetWalletId;
    public WalletId NewTargetWalletId { get; } = newTargetWalletId;

    public TransferAmount OldAmount { get; } = oldAmount;
    public TransferAmount NewAmount { get; } = newAmount;

    public IEnumerable<string> Tags { get; } = tags;
}
