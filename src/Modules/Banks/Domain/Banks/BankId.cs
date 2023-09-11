using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.Banks;

public class BankId : TypedIdValueBase
{
    public BankId(Guid value) : base(value)
    {
    }
}
