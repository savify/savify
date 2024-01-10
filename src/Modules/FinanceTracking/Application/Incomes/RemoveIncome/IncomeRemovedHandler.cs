using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;

public class IncomeRemovedHandler(IWalletsRepository walletsRepository) : INotificationHandler<IncomeRemovedDomainEvent>
{
    public async Task Handle(IncomeRemovedDomainEvent @event, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByWalletIdAsync(@event.WalletId);

        wallet.DecreaseBalance(@event.Amount);

        await walletsRepository.UpdateHistoryAsync(wallet);
    }
}
