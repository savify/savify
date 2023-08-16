using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;
using App.Modules.Wallets.Domain.Wallets.CashWallets;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;

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
        var cashWallet = CashWallet.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _cashWalletRepository.AddAsync(cashWallet);

        var viewMetadata = WalletViewMetadata.CreateForWallet(
            cashWallet.Id,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return cashWallet.Id.Value;
    }
}
