using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;

internal class EditCreditWalletCommandHandler : ICommandHandler<EditCreditWalletCommand>
{
    private readonly ICreditWalletRepository _creditWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public EditCreditWalletCommandHandler(ICreditWalletRepository creditWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _creditWalletRepository = creditWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task Handle(EditCreditWalletCommand command, CancellationToken cancellationToken)
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
    }
}
