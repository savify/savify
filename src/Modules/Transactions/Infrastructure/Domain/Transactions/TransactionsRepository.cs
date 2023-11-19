using App.Modules.Transactions.Domain.Transactions;

namespace App.Modules.Transactions.Infrastructure.Domain.Transactions;

internal class TransactionsRepository : ITransactionsRepository
{
    private readonly TransactionsContext _transactionsContext;

    public TransactionsRepository(TransactionsContext transactionsContext)
    {
        _transactionsContext = transactionsContext;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _transactionsContext.AddAsync(transaction);
    }
}
