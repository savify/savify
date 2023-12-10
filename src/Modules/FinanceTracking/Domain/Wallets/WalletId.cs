using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets;

public class WalletId : TypedIdValueBase
{
    public WalletId(Guid value) : base(value)
    {
    }
}
