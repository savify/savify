namespace App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;

public interface IAccountViewMetadataRepository
{
    Task AddAsync(AccountViewMetadata accountViewMetadata);

    Task<AccountViewMetadata> GetByAccountIdAsync(AccountId id);
}
