using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BankSynchronisationProcessing;

public class BankSynchronisationProcessId : TypedIdValueBase
{
    public BankSynchronisationProcessId(Guid value) : base(value)
    {
    }
}
