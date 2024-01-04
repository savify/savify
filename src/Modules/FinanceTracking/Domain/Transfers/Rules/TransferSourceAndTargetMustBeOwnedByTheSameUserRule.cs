using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Rules;

public class TransferSourceAndTargetMustBeOwnedByTheSameUserRule(
    UserId userId,
    WalletId sourceWalletId,
    WalletId targetWalletId,
    IWalletsRepository walletsRepository) : IBusinessRule
{
    public bool IsBroken() => !(walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId) &&
                              walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId));

    public string MessageTemplate => "Source and target wallets must be owned by the same user";
}
