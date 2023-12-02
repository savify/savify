using App.Modules.Transactions.Application.Contracts;

namespace App.Modules.Transactions.Application.Transactions.EditTransaction;

public class EditTransactionCommand : CommandBase
{
    public Guid TransactionId { get; }

    public TransactionTypeDto Type { get; }

    public SourceDto Source { get; }

    public TargetDto Target { get; }

    public DateTime MadeOn { get; }

    public string Comment { get; }

    public IEnumerable<string> Tags { get; }

    public EditTransactionCommand(Guid transactionId, TransactionTypeDto type, SourceDto source, TargetDto target, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        TransactionId = transactionId;
        Type = type;
        Source = source;
        Target = target;
        MadeOn = madeOn;
        Comment = comment;
        Tags = tags;
    }
}
