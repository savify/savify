namespace App.Modules.Transactions.Domain.Transactions;

public interface ITransactionsRepository
{
    public Task AddAsync(Transaction transaction);
}
