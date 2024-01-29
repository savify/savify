using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Incomes.Rules;

public class IncomeTargetWalletMustBeOwnedByUserRule(UserId userId, Wallet targetWallet) : IBusinessRule
{
    public bool IsBroken() => targetWallet.UserId != userId;

    public string MessageTemplate => "Target wallet must be owned by the user";
}
