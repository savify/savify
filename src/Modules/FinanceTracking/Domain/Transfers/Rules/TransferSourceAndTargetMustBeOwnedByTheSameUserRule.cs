using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Rules;

public class TransferSourceAndTargetMustBeOwnedByTheSameUserRule(
    UserId userId,
    Wallet sourceWallet,
    Wallet targetWallet) : IBusinessRule
{
    public bool IsBroken() => !(userId == sourceWallet.UserId && userId == targetWallet.UserId);

    public string MessageTemplate => "Source and target wallets must be owned by the same user";
}
