using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;

internal class RemoveCreditWalletCommandHandler(ICreditWalletRepository creditWalletRepository)
    : ICommandHandler<RemoveCreditWalletCommand>
{
    public async Task Handle(RemoveCreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await creditWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();
    }
}
