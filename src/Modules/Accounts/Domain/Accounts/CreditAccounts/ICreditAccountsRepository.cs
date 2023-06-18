namespace App.Modules.Accounts.Domain.Accounts.CreditAccounts;

public interface ICreditAccountsRepository
{
    Task AddAsync(CreditAccount account);

    Task<CreditAccount> GetByIdAsync(AccountId id);
}
