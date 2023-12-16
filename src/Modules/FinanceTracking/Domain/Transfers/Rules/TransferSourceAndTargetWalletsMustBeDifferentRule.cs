using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Rules;
public class TransferSourceAndTargetWalletsMustBeDifferentRule : IBusinessRule
{
    private readonly WalletId sourceWalletId;
    private readonly WalletId targetWalletId;

    public TransferSourceAndTargetWalletsMustBeDifferentRule(WalletId sourceWalletId, WalletId targetWalletId)
    {
        this.sourceWalletId = sourceWalletId;
        this.targetWalletId = targetWalletId;
    }

    public bool IsBroken()
    {
        return sourceWalletId == targetWalletId;
    }

    public string MessageTemplate => "Transfer has equal source and target wallets";
}
