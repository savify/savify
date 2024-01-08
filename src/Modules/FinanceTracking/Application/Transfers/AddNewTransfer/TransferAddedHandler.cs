﻿using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class TransferAddedHandler(IWalletsRepository walletsRepository) : INotificationHandler<TransferAddedDomainEvent>
{
    public async Task Handle(TransferAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var sourceWallet = await walletsRepository.GetByWalletIdAsync(@event.SourceWalletId);
        var targetWallet = await walletsRepository.GetByWalletIdAsync(@event.TargetWalletId);

        sourceWallet.DecreaseBalance(@event.Amount);
        targetWallet.IncreaseBalance(@event.Amount);

        await walletsRepository.UpdateHistoryAsync(sourceWallet);
        await walletsRepository.UpdateHistoryAsync(targetWallet);
    }
}
