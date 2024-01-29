using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;

public class TransferRemovedDomainEvent(TransferId transferId, WalletId sourceWalletId, WalletId targetWalletId, TransferAmount amount) : DomainEventBase
{
    public TransferId TransferId { get; } = transferId;

    public WalletId SourceWalletId { get; } = sourceWalletId;

    public WalletId TargetWalletId { get; } = targetWalletId;

    public TransferAmount Amount { get; } = amount;
}
