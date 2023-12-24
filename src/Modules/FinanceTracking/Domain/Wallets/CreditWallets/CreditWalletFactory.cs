using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public class CreditWalletFactory
{
    private readonly ICreditWalletRepository _creditWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public CreditWalletFactory(ICreditWalletRepository creditWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _creditWalletRepository = creditWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<CreditWallet> Create(
        UserId userId,
        string title,
        Currency currency,
        int creditLimit,
        int availableBalance,
        string color,
        string icon,
        bool considerInTotalBalance)
    {
        var wallet = CreditWallet.AddNew(
            userId,
            title,
            currency,
            creditLimit,
            availableBalance);

        await _creditWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.WalletViewMetadata.CreateForWallet(
            wallet.Id,
            color,
            icon,
            considerInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet;
    }
}
