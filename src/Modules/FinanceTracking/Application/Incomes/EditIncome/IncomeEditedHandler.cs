using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
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

            if (oldWallet is CashWallet oldCashWallet)
            {
                oldCashWallet.DecreaseBalance(@event.OldAmount);
            }

            if (oldWallet is DebitWallet oldDebitWallet)
            {
                oldDebitWallet.DecreaseBalance(@event.OldAmount);
            }

            if (oldWallet is CreditWallet oldCreditWallet)
            {
                oldCreditWallet.DecreaseAvailableBalance(@event.OldAmount);
            }

            if (newWallet is CashWallet newCashWallet)
            {
                newCashWallet.IncreaseBalance(@event.NewAmount);
            }

            if (newWallet is DebitWallet newDebitWallet)
            {
                newDebitWallet.IncreaseBalance(@event.NewAmount);
            }

            if (newWallet is CreditWallet newCreditWallet)
            {
                newCreditWallet.IncreaseAvailableBalance(@event.NewAmount);
            }

            await walletsRepository.UpdateHistoryAsync(oldWallet);
            await walletsRepository.UpdateHistoryAsync(newWallet);
        }
        else
        {
            var wallet = await walletsRepository.GetByWalletIdAsync(@event.OldTargetWalletId);

            if (wallet is CashWallet cashWallet)
            {
                cashWallet.DecreaseBalance(@event.OldAmount);
                cashWallet.IncreaseBalance(@event.NewAmount);
            }

            if (wallet is DebitWallet debitWallet)
            {
                debitWallet.DecreaseBalance(@event.OldAmount);
                debitWallet.IncreaseBalance(@event.NewAmount);
            }

            if (wallet is CreditWallet creditWallet)
            {
                creditWallet.DecreaseAvailableBalance(@event.OldAmount);
                creditWallet.IncreaseAvailableBalance(@event.NewAmount);
            }

            await walletsRepository.UpdateHistoryAsync(wallet);
        }
    }
}
