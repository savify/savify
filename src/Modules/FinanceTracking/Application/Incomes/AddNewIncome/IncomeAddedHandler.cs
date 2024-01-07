using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;

public class IncomeAddedHandler(IWalletsRepository walletsRepository) : INotificationHandler<IncomeAddedDomainEvent>
{
    public async Task Handle(IncomeAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByWalletIdAsync(@event.TargetWalletId);

        if (wallet is CashWallet cashWallet)
        {
            cashWallet.IncreaseBalance(@event.Amount);
        }

        if (wallet is DebitWallet debitWallet)
        {
            debitWallet.IncreaseBalance(@event.Amount);
        }

        if (wallet is CreditWallet creditWallet)
        {
            creditWallet.IncreaseAvailableBalance(@event.Amount);
        }

        await walletsRepository.UpdateHistoryAsync(wallet);
    }
}
