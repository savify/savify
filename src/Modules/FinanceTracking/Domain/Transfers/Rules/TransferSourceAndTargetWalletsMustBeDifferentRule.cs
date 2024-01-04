using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Rules;
public class TransferSourceAndTargetWalletsMustBeDifferentRule(WalletId sourceWalletId, WalletId targetWalletId) : IBusinessRule
{
    public bool IsBroken() => sourceWalletId == targetWalletId;

    public string MessageTemplate => "Transfer has equal source and target wallets";
}
