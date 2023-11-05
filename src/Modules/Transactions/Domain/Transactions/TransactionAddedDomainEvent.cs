using App.BuildingBlocks.Domain;

namespace App.Modules.Transactions.Domain.Transactions;

public class TransactionAddedDomainEvent: DomainEventBase
{
    public TransactionId TransactionId { get; }

    public TransactionType Type { get; }

    public Source Source { get; }

    public Target Target { get; }

    public TransactionAddedDomainEvent(TransactionId transactionId, TransactionType type, Source source, Target target)
    {
        TransactionId = transactionId;
        Type = type;
        Source = source;
        Target = target;
    }
}
