using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;

internal class TransferRemovedHandler(IWalletsRepository walletsRepository) : INotificationHandler<TransferRemovedDomainEvent>
{
    public async Task Handle(TransferRemovedDomainEvent @event, CancellationToken cancellationToken)
    {
        var sourceWallet = await walletsRepository.GetByWalletIdAsync(@event.SourceWalletId);
        var targetWallet = await walletsRepository.GetByWalletIdAsync(@event.TargetWalletId);

        sourceWallet.IncreaseBalance(@event.Amount);
        targetWallet.DecreaseBalance(@event.Amount);

        await walletsRepository.UpdateHistoryAsync(sourceWallet);
        await walletsRepository.UpdateHistoryAsync(targetWallet);
    }
}
