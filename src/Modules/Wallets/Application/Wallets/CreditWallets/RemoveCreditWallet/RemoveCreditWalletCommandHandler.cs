using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.RemoveCreditWallet;

internal class RemoveCreditWalletCommandHandler : ICommandHandler<RemoveCreditWalletCommand, Result>
{
    private readonly ICreditWalletRepository _creditWalletRepository;

    public RemoveCreditWalletCommandHandler(ICreditWalletRepository creditWalletRepository)
    {
        _creditWalletRepository = creditWalletRepository;
    }

    public async Task<Result> Handle(RemoveCreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _creditWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();

        return Result.Success;
    }
}
