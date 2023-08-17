using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CashWallets;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.RemoveCashWallet;

public class RemoveCashWalletCommandHandler : ICommandHandler<RemoveCashWalletCommand, Result>
{
    private readonly ICashWalletRepository _cashWalletRepository;

    public RemoveCashWalletCommandHandler(ICashWalletRepository cashWalletRepository)
    {
        _cashWalletRepository = cashWalletRepository;
    }

    public async Task<Result> Handle(RemoveCashWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _cashWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();

        return Result.Success;
    }
}
