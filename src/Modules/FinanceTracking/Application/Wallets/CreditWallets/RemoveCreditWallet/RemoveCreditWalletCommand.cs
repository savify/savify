using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;

public class RemoveCreditWalletCommand(Guid userId, Guid walletId) : CommandBase
{
    public Guid UserId { get; } = userId;

    public Guid WalletId { get; } = walletId;
}
