using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class TransferAddedHandler(IWalletsRepository walletsRepository) : INotificationHandler<TransferAddedDomainEvent>
{
    public async Task Handle(TransferAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var sourceWallet = await walletsRepository.GetByWalletIdAsync(@event.SourceWalletId);
        sourceWallet.DecreaseBalance(@event.Amount.Source);
        await walletsRepository.UpdateHistoryAsync(sourceWallet);

        var targetWallet = await walletsRepository.GetByWalletIdAsync(@event.TargetWalletId);
        targetWallet.IncreaseBalance(@event.Amount.Target);
        await walletsRepository.UpdateHistoryAsync(targetWallet);
    }
}
