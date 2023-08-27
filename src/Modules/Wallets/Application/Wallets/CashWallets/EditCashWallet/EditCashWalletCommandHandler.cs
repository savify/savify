using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CashWallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.EditCashWallet;

public class EditCashWalletCommandHandler : ICommandHandler<EditCashWalletCommand, Result>
{
    private readonly ICashWalletRepository _cashWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public EditCashWalletCommandHandler(ICashWalletRepository cashWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _cashWalletRepository = cashWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Result> Handle(EditCashWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _cashWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));
        wallet.Edit(
            command.Title,
            command.Currency != null ? new Currency(command.Currency) : null,
            command.Balance);

        var walletViewMetadata = await _walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        return Result.Success;
    }
}
