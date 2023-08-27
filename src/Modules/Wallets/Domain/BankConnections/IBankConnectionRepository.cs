namespace App.Modules.Wallets.Domain.BankConnections;

public interface IBankConnectionRepository
{
    Task AddAsync(BankConnection bankConnection);

    Task<BankConnection> GetByIdAsync(BankConnectionId id);

    Task RemoveByIdAsync(BankConnectionId id);
}
