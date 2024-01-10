using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

public class DebitWalletEditor(
    IDebitWalletRepository debitWalletRepository,
    IWalletViewMetadataRepository walletViewMetadataRepository)
{
    public async Task EditWallet(
        UserId userId,
        WalletId walletId,
        string? title,
        int? balance,
        string? color,
        string? icon,
        bool? considerInTotalBalance)
    {
        var wallet = await debitWalletRepository.GetByIdAndUserIdAsync(walletId, userId);

        if (title is not null) wallet.ChangeTitle(title);
        if (balance is not null && balance != wallet.Balance) wallet.ChangeBalance((int)balance);

        await debitWalletRepository.UpdateHistoryAsync(wallet);

        var walletViewMetadata = await walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(color, icon, considerInTotalBalance);
    }
}
