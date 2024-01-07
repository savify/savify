using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets;

public interface IDebitWalletReadRepository
{
    Task<int> GetBalanceAsync(WalletId id);
}
