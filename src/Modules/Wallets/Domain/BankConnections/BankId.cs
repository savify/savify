using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnections;

public class BankId : TypedIdValueBase
{
    public BankId(Guid value) : base(value)
    {
    }
}
