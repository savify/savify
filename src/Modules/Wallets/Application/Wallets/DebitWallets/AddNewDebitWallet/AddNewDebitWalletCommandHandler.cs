using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;

internal class AddNewDebitWalletCommandHandler : ICommandHandler<AddNewDebitWalletCommand, Guid>
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public AddNewDebitWalletCommandHandler(IDebitWalletRepository debitWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _debitWalletRepository = debitWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = DebitWallet.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _debitWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.CreateDefaultForWallet(wallet.Id);
        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet.Id.Value;
    }
}
