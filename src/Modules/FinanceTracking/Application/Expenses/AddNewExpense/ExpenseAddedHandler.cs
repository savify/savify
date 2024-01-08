using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;

public class ExpenseAddedHandler(IWalletsRepository walletsRepository) : INotificationHandler<ExpenseAddedDomainEvent>
{
    public async Task Handle(ExpenseAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByWalletIdAsync(@event.SourceWalletId);

        wallet.DecreaseBalance(@event.Amount);

        await walletsRepository.UpdateHistoryAsync(wallet);
    }
}
