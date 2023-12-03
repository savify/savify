using App.BuildingBlocks.Infrastructure.Exceptions;
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

    public async Task<Transaction> GetByIdAsync(TransactionId id)
    {
        var transaction = await _transactionsContext.Transactions.FindAsync(id);
        if (transaction is null)
        {
            throw new NotFoundRepositoryException<Transaction>(id.Value);
        }

        return transaction;
    }

    public void Remove(Transaction transaction)
    {
        _transactionsContext.Remove(transaction);
    }
}
