using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;
using Currency = App.Modules.FinanceTracking.Domain.Finance.Currency;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;

internal class AddNewCashWalletCommandHandler : ICommandHandler<AddNewCashWalletCommand, Guid>
{
    private readonly ICashWalletRepository _cashWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public AddNewCashWalletCommandHandler(ICashWalletRepository cashWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _cashWalletRepository = cashWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCashWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = CashWallet.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _cashWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.CreateForWallet(
            wallet.Id,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet.Id.Value;
    }
}
