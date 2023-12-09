using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.RemoveDebitWallet;

internal class RemoveDebitWalletCommandHandler : ICommandHandler<RemoveDebitWalletCommand>
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    public RemoveDebitWalletCommandHandler(IDebitWalletRepository debitWalletRepository)
    {
        _debitWalletRepository = debitWalletRepository;
    }

    public async Task Handle(RemoveDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _debitWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        wallet.Remove();
    }
}
