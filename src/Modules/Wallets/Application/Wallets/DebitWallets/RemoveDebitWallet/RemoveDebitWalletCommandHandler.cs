using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.RemoveDebitWallet;

public class RemoveDebitWalletCommandHandler : ICommandHandler<RemoveDebitWalletCommand, Result>
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    public RemoveDebitWalletCommandHandler(IDebitWalletRepository debitWalletRepository)
    {
        _debitWalletRepository = debitWalletRepository;
    }

    public async Task<Result> Handle(RemoveDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _debitWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();

        return Result.Success;
    }
}
