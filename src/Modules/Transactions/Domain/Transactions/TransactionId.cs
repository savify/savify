using App.BuildingBlocks.Domain;

namespace App.Modules.Transactions.Domain.Transactions;

public class TransactionId : TypedIdValueBase
{
    public TransactionId(Guid value) : base(value)
    {
    }
}
