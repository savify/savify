using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Expenses.EditExpense;

public class ExpenseEditedHandler(IWalletsRepository walletsRepository) : INotificationHandler<ExpenseEditedDomainEvent>
{
    public async Task Handle(ExpenseEditedDomainEvent @event, CancellationToken cancellationToken)
    {
        if (@event.NewSourceWalletId != @event.OldSourceWalletId)
        {
            var oldWallet = await walletsRepository.GetByWalletIdAsync(@event.OldSourceWalletId);
            var newWallet = await walletsRepository.GetByWalletIdAsync(@event.NewSourceWalletId);

            oldWallet.IncreaseBalance(@event.OldAmount);
            newWallet.DecreaseBalance(@event.NewAmount);

            await walletsRepository.UpdateHistoryAsync(oldWallet);
            await walletsRepository.UpdateHistoryAsync(newWallet);
        }
        else
        {
            var wallet = await walletsRepository.GetByWalletIdAsync(@event.OldSourceWalletId);

            wallet.IncreaseBalance(@event.OldAmount);
            wallet.DecreaseBalance(@event.NewAmount);

            await walletsRepository.UpdateHistoryAsync(wallet);
        }
    }
}
