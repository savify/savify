using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.Transactions.Infrastructure.Outbox;

public class Outbox : IOutbox
{
    private readonly TransactionsContext _transactionsContext;

    public Outbox(TransactionsContext transactionsContext)
    {
        _transactionsContext = transactionsContext;
    }

    public void Add(OutboxMessage message)
    {
        _transactionsContext.OutboxMessages?.Add(message);
    }
}
