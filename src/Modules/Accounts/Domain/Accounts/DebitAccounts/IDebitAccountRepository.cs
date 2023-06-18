namespace App.Modules.Accounts.Domain.Accounts.DebitAccounts
{
    public interface IDebitAccountRepository
    {
        Task AddAsync(DebitAccount account);

        Task<DebitAccount> GetByIdAsync(AccountId id);
    }
}
