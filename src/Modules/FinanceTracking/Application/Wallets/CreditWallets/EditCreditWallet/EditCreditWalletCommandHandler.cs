using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;

internal class EditCreditWalletCommandHandler(CreditWalletEditor creditWalletEditor) : ICommandHandler<EditCreditWalletCommand>
{
    public Task Handle(EditCreditWalletCommand command, CancellationToken cancellationToken)
    {
        return creditWalletEditor.EditWallet(
            new UserId(command.UserId),
            new WalletId(command.WalletId),
            command.Title,
            command.AvailableBalance,
            command.CreditLimit,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);
    }
}
