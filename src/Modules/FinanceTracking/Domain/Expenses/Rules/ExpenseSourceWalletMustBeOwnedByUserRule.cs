using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Expenses.Rules;

public class ExpenseSourceWalletMustBeOwnedByUserRule(UserId userId, Wallet sourceWallet) : IBusinessRule
{
    public bool IsBroken() => sourceWallet.UserId != userId;

    public string MessageTemplate => "Source wallet must be owned by the user";
}
