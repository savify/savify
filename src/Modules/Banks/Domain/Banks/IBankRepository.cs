namespace App.Modules.Banks.Domain.Banks;

public interface IBankRepository
{
    public Task AddAsync(Bank bank);

    public Task<Bank> GetByIdAsync(BankId id);

    public Task<Bank?> GetByExternalIdAsync(string externalId);
}
