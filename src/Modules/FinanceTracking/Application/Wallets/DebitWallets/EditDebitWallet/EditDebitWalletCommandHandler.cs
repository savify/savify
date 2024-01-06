using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;

internal class EditDebitWalletCommandHandler(DebitWalletEditionService debitWalletEditionService) : ICommandHandler<EditDebitWalletCommand>
{
    public Task Handle(EditDebitWalletCommand command, CancellationToken cancellationToken)
    {
        return debitWalletEditionService.EditWallet(
            new UserId(command.UserId),
            new WalletId(command.WalletId),
            command.Title,
            command.Balance,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);
    }
}
