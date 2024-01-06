using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public class CreditWalletEditionService(
    ICreditWalletRepository creditWalletRepository,
    IWalletViewMetadataRepository walletViewMetadataRepository)
{
    public async Task EditWallet(
        UserId userId,
        WalletId walletId,
        string? title,
        int? availableBalance,
        int? creditLimit,
        string? color,
        string? icon,
        bool? considerInTotalBalance)
    {
        var wallet = await creditWalletRepository.GetByIdAndUserIdAsync(walletId, userId);
        wallet.Edit(title, availableBalance, creditLimit);

        var walletViewMetadata = await walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(color, icon, considerInTotalBalance);
    }
}
