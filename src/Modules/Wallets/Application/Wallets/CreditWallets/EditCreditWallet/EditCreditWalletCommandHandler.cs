using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.EditCreditWallet;

internal class EditCreditWalletCommandHandler : ICommandHandler<EditCreditWalletCommand, Result>
{
    private readonly ICreditWalletRepository _creditWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public EditCreditWalletCommandHandler(ICreditWalletRepository creditWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _creditWalletRepository = creditWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Result> Handle(EditCreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _creditWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));
        wallet.Edit(
            command.Title,
            command.Currency != null ? new Currency(command.Currency) : null,
            command.AvailableBalance,
            command.CreditLimit);

        var walletViewMetadata = await _walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        return Result.Success;
    }
}
