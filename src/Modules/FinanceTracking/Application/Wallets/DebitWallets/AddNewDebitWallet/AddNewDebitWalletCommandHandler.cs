using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;

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

        var viewMetadata = WalletViewMetadata.CreateForWallet(
            wallet.Id,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet.Id.Value;
    }
}
