using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.AccountViewMetadata;

public class AccountViewMetadataRepository : IAccountViewMetadataRepository
{
    private readonly AccountsContext _accountsContext;

    public AccountViewMetadataRepository(AccountsContext accountsContext)
    {
        _accountsContext = accountsContext;
    }

    public async Task AddAsync(Modules.Accounts.Domain.Accounts.AccountViewMetadata.AccountViewMetadata accountViewMetadata)
    {
        await _accountsContext.AddAsync(accountViewMetadata);
    }

    public async Task<Modules.Accounts.Domain.Accounts.AccountViewMetadata.AccountViewMetadata> GetByAccountIdAsync(AccountId accountId)
    {
        var accountViewMetadata = await _accountsContext.AccountViewMetadata.FirstOrDefaultAsync(x => x.AccountId == accountId);

        if (accountViewMetadata == null)
        {
            throw new NotFoundRepositoryException<Modules.Accounts.Domain.Accounts.AccountViewMetadata.AccountViewMetadata>(
                "AccountViewMetadata for Account with id '{0}' was not found",
                new object[]{ accountId.Value });
        }
        
        return accountViewMetadata;
    }
}