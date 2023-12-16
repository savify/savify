using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public class CreditWalletEditionService
{
    private readonly ICreditWalletRepository _creditWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public CreditWalletEditionService(ICreditWalletRepository creditWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _creditWalletRepository = creditWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task EditWallet(
        UserId userId,
        WalletId walletId,
        string? title,
        Currency? currency,
        int? availableBalance,
        int? creditLimit,
        string? color,
        string? icon,
        bool? considerInTotalBalance)
    {
        var wallet = await _creditWalletRepository.GetByIdAndUserIdAsync(walletId, userId);
        wallet.Edit(title, currency, availableBalance, creditLimit);

        var walletViewMetadata = await _walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(color, icon, considerInTotalBalance);
    }
}
