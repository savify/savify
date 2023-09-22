using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationProcessId : TypedIdValueBase
{
    public BanksSynchronisationProcessId(Guid value) : base(value)
    {
    }
}
