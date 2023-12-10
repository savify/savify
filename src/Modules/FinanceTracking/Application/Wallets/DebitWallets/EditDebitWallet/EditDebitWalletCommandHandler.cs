using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;

internal class EditDebitWalletCommandHandler : ICommandHandler<EditDebitWalletCommand>
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public EditDebitWalletCommandHandler(IDebitWalletRepository debitWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _debitWalletRepository = debitWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task Handle(EditDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _debitWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));
        wallet.Edit(
            command.Title,
            command.Currency != null ? new Currency(command.Currency) : null,
            command.Balance);

        var walletViewMetadata = await _walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);
    }
}
