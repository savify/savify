using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.Banks.BankRevisions;

public class BankRevisionId : TypedIdValueBase
{
    public BankRevisionId(Guid value) : base(value)
    {
    }
}
