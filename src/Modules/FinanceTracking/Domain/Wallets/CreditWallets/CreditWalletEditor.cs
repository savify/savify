using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public class CreditWalletEditor(
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

        if (title is not null) wallet.ChangeTitle(title);
        if (creditLimit is not null && creditLimit != wallet.CreditLimit) wallet.ChangeCreditLimit((int)creditLimit);
        if (availableBalance is not null && availableBalance != wallet.AvailableBalance) wallet.ChangeAvailableBalance((int)availableBalance);

        await creditWalletRepository.UpdateHistoryAsync(wallet);

        var walletViewMetadata = await walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(color, icon, considerInTotalBalance);
    }
}
