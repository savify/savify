using App.Modules.Transactions.Application.Contracts;

namespace App.Modules.Transactions.Application.Transactions.AddNewTransaction;

public class AddNewTransactionCommand : CommandBase<Guid>
{
    public TransactionTypeDto Type { get; }

    public SourceDto Source { get; }

    public TargetDto Target { get; }

    public DateTime MadeOn { get; }

    public string Comment { get; }

    public IEnumerable<string> Tags { get; }

    public AddNewTransactionCommand(TransactionTypeDto type, SourceDto source, TargetDto target, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        Type = type;
        Source = source;
        Target = target;
        MadeOn = madeOn;
        Comment = comment;
        Tags = tags;
    }
}
