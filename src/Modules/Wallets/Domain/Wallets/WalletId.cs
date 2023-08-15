using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Wallets;

public class WalletId : TypedIdValueBase
{
    public WalletId(Guid value) : base(value)
    {
    }
}
