using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

public class CashWalletFactory
{
    private readonly ICashWalletRepository _cashWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public CashWalletFactory(ICashWalletRepository cashWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _cashWalletRepository = cashWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<CashWallet> Create(UserId userId, string title, Currency currency, int balance, string color, string icon, bool considerInTotalBalance)
    {
        var wallet = CashWallet.AddNew(
            userId,
            title,
            currency,
            balance);

        await _cashWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.WalletViewMetadata.CreateForWallet(
            wallet.Id,
            color,
            icon,
            considerInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet;
    }
}
