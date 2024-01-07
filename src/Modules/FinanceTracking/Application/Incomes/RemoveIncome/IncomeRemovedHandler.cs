using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;

public class IncomeRemovedHandler(IWalletsRepository walletsRepository) : INotificationHandler<IncomeRemovedDomainEvent>
{
    public async Task Handle(IncomeRemovedDomainEvent @event, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByWalletIdAsync(@event.WalletId);

        if (wallet is CashWallet cashWallet)
        {
            cashWallet.DecreaseBalance(@event.Amount);
        }

        if (wallet is DebitWallet debitWallet)
        {
            debitWallet.DecreaseBalance(@event.Amount);
        }

        if (wallet is CreditWallet creditWallet)
        {
            creditWallet.DecreaseAvailableBalance(@event.Amount);
        }

        await walletsRepository.UpdateHistoryAsync(wallet);
    }
}
