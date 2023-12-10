using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;

internal class RemoveCreditWalletCommandHandler : ICommandHandler<RemoveCreditWalletCommand>
{
    private readonly ICreditWalletRepository _creditWalletRepository;

    public RemoveCreditWalletCommandHandler(ICreditWalletRepository creditWalletRepository)
    {
        _creditWalletRepository = creditWalletRepository;
    }

    public async Task Handle(RemoveCreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _creditWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();
    }
}
