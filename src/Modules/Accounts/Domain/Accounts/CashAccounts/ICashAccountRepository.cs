namespace App.Modules.Accounts.Domain.Accounts.CashAccounts;

public interface ICashAccountRepository
{
    Task AddAsync(CashAccount account);

    Task<CashAccount> GetByIdAsync(AccountId id);
}
