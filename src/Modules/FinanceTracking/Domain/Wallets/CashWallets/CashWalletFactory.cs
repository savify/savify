using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

public class CashWalletFactory(
    ICashWalletRepository cashWalletRepository,
    IWalletViewMetadataRepository walletViewMetadataRepository)
{
    public async Task<CashWallet> Create(UserId userId, string title, Currency currency, int initialBalance, string color, string icon, bool considerInTotalBalance)
    {
        var wallet = CashWallet.AddNew(
            userId,
            title,
            currency,
            initialBalance);

        await cashWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.WalletViewMetadata.CreateForWallet(
            wallet.Id,
            color,
            icon,
            considerInTotalBalance);

        await walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet;
    }
}
