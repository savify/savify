using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

internal class TransferEditedHandler(IWalletsRepository walletsRepository) : INotificationHandler<TransferEditedDomainEvent>
{
    public async Task Handle(TransferEditedDomainEvent @event, CancellationToken cancellationToken)
    {
        var sourceWalletWasChanged = @event.NewSourceWalletId != @event.OldSourceWalletId;
        var targetWalletWasChanged = @event.NewTargetWalletId != @event.OldTargetWalletId;

        if (!sourceWalletWasChanged && !targetWalletWasChanged)
        {
            await UpdateBalancesWhenWalletsHaveChanged(@event);
        }
        else if (sourceWalletWasChanged && targetWalletWasChanged)
        {
            await UpdateBalancesWhenWalletsChanged(@event);
        }
        else
        {
            if (sourceWalletWasChanged)
            {
                await UpdateBalancesWhenSourceWalletChanged(@event);
            }

            if (targetWalletWasChanged)
            {
                await UpdateBalancesWhenTargetWalletChanged(@event);
            }
        }
    }

    private async Task UpdateBalancesWhenWalletsHaveChanged(TransferEditedDomainEvent @event)
    {
        var sourceWallet = await walletsRepository.GetByWalletIdAsync(@event.OldSourceWalletId);
        var targetWallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);

        sourceWallet.IncreaseBalance(@event.OldAmount.Source);
        sourceWallet.DecreaseBalance(@event.NewAmount.Source);

        targetWallet.DecreaseBalance(@event.OldAmount.Target);
        targetWallet.IncreaseBalance(@event.NewAmount.Target);

        await walletsRepository.UpdateHistoryAsync(sourceWallet);
        await walletsRepository.UpdateHistoryAsync(targetWallet);
    }

    private async Task UpdateBalancesWhenWalletsChanged(TransferEditedDomainEvent @event)
    {
        var oldSourceWallet = await walletsRepository.GetByWalletIdAsync(@event.OldSourceWalletId);
        var newSourceWallet = await walletsRepository.GetByWalletIdAsync(@event.NewSourceWalletId);
        var oldTargetWallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);
        var newTargetWallet = await walletsRepository.GetByWalletIdAsync(@event.NewTargetWalletId);

        oldSourceWallet.IncreaseBalance(@event.OldAmount.Source);
        newSourceWallet.DecreaseBalance(@event.NewAmount.Source);

        oldTargetWallet.DecreaseBalance(@event.OldAmount.Target);
        newTargetWallet.IncreaseBalance(@event.NewAmount.Target);

        await walletsRepository.UpdateHistoryAsync(oldSourceWallet);
        await walletsRepository.UpdateHistoryAsync(newSourceWallet);
        await walletsRepository.UpdateHistoryAsync(oldTargetWallet);
        await walletsRepository.UpdateHistoryAsync(newTargetWallet);
    }

    private async Task UpdateBalancesWhenSourceWalletChanged(TransferEditedDomainEvent @event)
    {
        var oldSourceWallet = await walletsRepository.GetByWalletIdAsync(@event.OldSourceWalletId);
        var newSourceWallet = await walletsRepository.GetByWalletIdAsync(@event.NewSourceWalletId);
        var targetWallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);

        oldSourceWallet.IncreaseBalance(@event.OldAmount.Source);
        newSourceWallet.DecreaseBalance(@event.NewAmount.Source);

        targetWallet.DecreaseBalance(@event.OldAmount.Target);
        targetWallet.IncreaseBalance(@event.NewAmount.Target);

        await walletsRepository.UpdateHistoryAsync(oldSourceWallet);
        await walletsRepository.UpdateHistoryAsync(newSourceWallet);
        await walletsRepository.UpdateHistoryAsync(targetWallet);
    }

    private async Task UpdateBalancesWhenTargetWalletChanged(TransferEditedDomainEvent @event)
    {
        var sourceWallet = await walletsRepository.GetByWalletIdAsync(@event.OldSourceWalletId);
        var oldTargetWallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);
        var newTargetWallet = await walletsRepository.GetByWalletIdAsync(@event.NewTargetWalletId);

        sourceWallet.IncreaseBalance(@event.OldAmount.Source);
        sourceWallet.DecreaseBalance(@event.NewAmount.Source);

        oldTargetWallet.DecreaseBalance(@event.OldAmount.Target);
        newTargetWallet.IncreaseBalance(@event.NewAmount.Target);

        await walletsRepository.UpdateHistoryAsync(sourceWallet);
        await walletsRepository.UpdateHistoryAsync(oldTargetWallet);
        await walletsRepository.UpdateHistoryAsync(newTargetWallet);
    }
}
