using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets;

public interface ICreditWalletReadRepository
{
    Task<int> GetAvailableBalanceAsync(WalletId id);
}
