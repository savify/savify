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
    Money newAmount) : DomainEventBase
{
    public WalletId OldSourceWalletId { get; } = oldSourceWalletId;
    public WalletId NewSourceWalletId { get; } = newSourceWalletId;

    public WalletId OldTargetWalletId { get; } = oldTargetWalletId;
    public WalletId NewTargetWalletId { get; } = newTargetWalletId;

    public Money OldAmount { get; } = oldAmount;
    public Money NewAmount { get; } = newAmount;
}
