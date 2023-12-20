using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.RemoveCashWallet;

internal class RemoveCashWalletCommandHandler(ICashWalletRepository cashWalletRepository)
    : ICommandHandler<RemoveCashWalletCommand>
{
    public async Task Handle(RemoveCashWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await cashWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();
    }
}
