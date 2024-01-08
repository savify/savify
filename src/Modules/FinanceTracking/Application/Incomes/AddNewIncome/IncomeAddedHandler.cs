using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;

public class IncomeAddedHandler(IWalletsRepository walletsRepository) : INotificationHandler<IncomeAddedDomainEvent>
{
    public async Task Handle(IncomeAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByWalletIdAsync(@event.TargetWalletId);

        wallet.IncreaseBalance(@event.Amount);

        await walletsRepository.UpdateHistoryAsync(wallet);
    }
}
