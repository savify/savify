using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.EditCashWallet;

internal class EditCashWalletCommandHandler(CashWalletEditionService cashWalletEditionService) : ICommandHandler<EditCashWalletCommand>
{
    public Task Handle(EditCashWalletCommand command, CancellationToken cancellationToken)
    {
        return cashWalletEditionService.EditWallet(
            new UserId(command.UserId),
            new WalletId(command.WalletId),
            command.Title,
            command.Balance,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);
    }
}
