using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets;

public interface ICashWalletReadRepository
{
    Task<int> GetBalanceAsync(WalletId id);
}
