using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.EditDebitWallet;

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
