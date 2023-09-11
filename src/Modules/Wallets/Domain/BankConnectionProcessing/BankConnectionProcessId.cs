using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public class BankConnectionProcessId : TypedIdValueBase
{
    public BankConnectionProcessId(Guid value) : base(value)
    {
    }
}
