namespace App.Modules.Transactions.Application.Transactions;

public class TransactionDto
{
    public Guid Id { get; internal set; }

    public TransactionTypeDto Type { get; internal set; }

    public SourceDto Source { get; internal set; }

    public TargetDto Target { get; internal set; }

    public DateTime MadeOn { get; internal set; }

    public string Comment { get; internal set; }

    public IEnumerable<string> Tags { get; set; }
}
