using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.EditIncome;

public class IncomeEditedHandler(IWalletsRepository walletsRepository) : INotificationHandler<IncomeEditedDomainEvent>
{
    public async Task Handle(IncomeEditedDomainEvent @event, CancellationToken cancellationToken)
    {
        if (@event.NewTargetWalletId != @event.OldTargetWalletId)
        {
            var oldWallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);
            var newWallet = await walletsRepository.GetByWalletIdAsync(@event.NewTargetWalletId);

            oldWallet.DecreaseBalance(@event.OldAmount);
            newWallet.IncreaseBalance(@event.NewAmount);

            await walletsRepository.UpdateHistoryAsync(oldWallet);
            await walletsRepository.UpdateHistoryAsync(newWallet);
        }
        else
        {
            var wallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);

            wallet.DecreaseBalance(@event.OldAmount);
            wallet.IncreaseBalance(@event.NewAmount);

            await walletsRepository.UpdateHistoryAsync(wallet);
        }
    }
}
