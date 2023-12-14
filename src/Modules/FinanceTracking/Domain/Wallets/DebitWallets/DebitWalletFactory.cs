using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

public class DebitWalletFactory
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public DebitWalletFactory(IDebitWalletRepository debitWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _debitWalletRepository = debitWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<DebitWallet> Create(UserId userId, string title, Currency currency, int balance, string color, string icon, bool considerInTotalBalance)
    {
        var wallet = DebitWallet.AddNew(
            userId,
            title,
            currency,
            balance);

        await _debitWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.WalletViewMetadata.CreateForWallet(
            wallet.Id,
            color,
            icon,
            considerInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet;
    }
}
