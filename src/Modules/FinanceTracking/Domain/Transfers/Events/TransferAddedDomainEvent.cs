﻿using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;

public class TransferAddedDomainEvent(TransferId transferId, UserId userId, WalletId sourceWalletId, WalletId targetWalletId, TransferAmount amount, IEnumerable<string> tags) : DomainEventBase
{
    public TransferId TransferId { get; } = transferId;

    public UserId UserId { get; } = userId;

    public WalletId SourceWalletId { get; } = sourceWalletId;

    public WalletId TargetWalletId { get; } = targetWalletId;

    public TransferAmount Amount { get; } = amount;

    public IEnumerable<string> Tags { get; } = tags;
}
