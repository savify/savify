using App.BuildingBlocks.Domain;

namespace App.Modules.Transactions.Domain.Transactions.Events;

public class TransactionEditedDomainEvent : DomainEventBase
{
    public TransactionId TransactionId { get; }

    public TransactionType Type { get; }

    public Source OldSource { get; }

    public Source NewSource { get; }

    public Target OldTarget { get; }

    public Target NewTarget { get; }

    public TransactionEditedDomainEvent(TransactionId transactionId, TransactionType type, Source oldSource, Source newSource, Target oldTarget, Target newTarget)
    {
        TransactionId = transactionId;
        Type = type;
        OldSource = oldSource;
        NewSource = newSource;
        OldTarget = oldTarget;
        NewTarget = newTarget;
    }
}

