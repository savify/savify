using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.EditCashWallet;

internal class EditCashWalletCommandHandler(CashWalletEditingService cashWalletEditingService) : ICommandHandler<EditCashWalletCommand>
{
    public async Task Handle(EditCashWalletCommand command, CancellationToken cancellationToken)
    {
        await cashWalletEditingService.EditWallet(
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
