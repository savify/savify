using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;
public class TransferEditedDomainEvent(
    WalletId oldSourceWalletId,
    WalletId newSourceWalletId,
    WalletId oldTargetWalletId,
    WalletId newTargetWalletId,
    Money oldAmount,
    Money newAmount,
    DateTime oldMadeOn,
    DateTime newMadeOn,
    string oldComment,
    string newComment,
    IEnumerable<string> oldTags,
    IEnumerable<string> newTags) : DomainEventBase
{
    public WalletId OldSourceWalletId { get; } = oldSourceWalletId;
    public WalletId NewSourceWalletId { get; } = newSourceWalletId;

    public WalletId OldTargetWalletId { get; } = oldTargetWalletId;
    public WalletId NewTargetWalletId { get; } = newTargetWalletId;

    public Money OldAmount { get; } = oldAmount;
    public Money NewAmount { get; } = newAmount;

    public DateTime OldMadeOn { get; } = oldMadeOn;
    public DateTime NewMadeOn { get; } = newMadeOn;

    public string OldComment { get; } = oldComment;
    public string NewComment { get; } = newComment;

    public IEnumerable<string> OldTags { get; } = oldTags;
    public IEnumerable<string> NewTags { get; } = newTags;
}
