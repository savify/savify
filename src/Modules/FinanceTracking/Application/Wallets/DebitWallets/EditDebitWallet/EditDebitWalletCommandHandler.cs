using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;

internal class EditDebitWalletCommandHandler(DebitWalletEditingService debitWalletEditingService) : ICommandHandler<EditDebitWalletCommand>
{
    public Task Handle(EditDebitWalletCommand command, CancellationToken cancellationToken)
    {
        return debitWalletEditingService.EditWallet(
            new UserId(command.UserId),
            new WalletId(command.WalletId),
            command.Title,
            command.Currency != null ? new Currency(command.Currency) : null,
            command.Balance,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);
    }
}
