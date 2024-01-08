using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;

public class ExpenseRemovedHandler(IWalletsRepository walletsRepository) : INotificationHandler<ExpenseRemovedDomainEvent>
{
    public async Task Handle(ExpenseRemovedDomainEvent @event, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByWalletIdAsync(@event.SourceWalletId);

        wallet.IncreaseBalance(@event.Amount);

        await walletsRepository.UpdateHistoryAsync(wallet);
    }
}
