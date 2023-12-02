namespace App.Modules.Transactions.Domain.Transactions;

public interface ITransactionsRepository
{
    Task AddAsync(Transaction transaction);

    Task<Transaction> GetByIdAsync(TransactionId id);
}
