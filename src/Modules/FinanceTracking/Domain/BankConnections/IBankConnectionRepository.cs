namespace App.Modules.FinanceTracking.Domain.BankConnections;

public interface IBankConnectionRepository
{
    Task AddAsync(BankConnection bankConnection);

    Task<BankConnection> GetByIdAsync(BankConnectionId id);

    void Remove(BankConnection bankConnection);
}
