using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnections;

public class BankConnectionId : TypedIdValueBase
{
    public BankConnectionId(Guid value) : base(value)
    {
    }
}
