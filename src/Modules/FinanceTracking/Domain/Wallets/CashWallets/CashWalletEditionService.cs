using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

public class CashWalletEditionService
{
    private readonly ICashWalletRepository _cashWalletRepository;

    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public CashWalletEditionService(ICashWalletRepository cashWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _cashWalletRepository = cashWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task EditWallet(
        UserId userId,
        WalletId walletId,
        string? title,
        Currency? currency,
        int? balance,
        string? color,
        string? icon,
        bool? considerInTotalBalance)
    {
        var wallet = await _cashWalletRepository.GetByIdAndUserIdAsync(walletId, userId);
        wallet.Edit(title, currency, balance);

        var walletViewMetadata = await _walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id);
        walletViewMetadata.Edit(color, icon, considerInTotalBalance);
    }
}
