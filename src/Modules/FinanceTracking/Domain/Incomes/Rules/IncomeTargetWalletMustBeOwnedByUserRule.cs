using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Incomes.Rules;

public class IncomeTargetWalletMustBeOwnedByUserRule(UserId userId, WalletId targetWalletId, IWalletsRepository walletsRepository) : IBusinessRule
{
    public bool IsBroken() => !walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId);

    public string MessageTemplate => "Target wallet must be owned by the user";
}
