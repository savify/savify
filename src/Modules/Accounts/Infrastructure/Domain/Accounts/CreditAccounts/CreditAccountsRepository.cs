using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.CreditAccounts;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.CreditAccounts;

internal class CreditAccountsRepository : ICreditAccountsRepository
{
    private readonly AccountsContext _accountsContext;

    public CreditAccountsRepository(AccountsContext accountsContext)
    {
        _accountsContext = accountsContext;
    }

    public async Task AddAsync(CreditAccount account)
    {
        await _accountsContext.AddAsync(account);
    }

    public async Task<CreditAccount> GetByIdAsync(AccountId id)
    {
        var account = await _accountsContext.CreditAccounts.FirstOrDefaultAsync(account => account.Id == id);

        if (account is null)
        {
            throw new NotFoundRepositoryException<CreditAccount>(id.Value);
        }

        return account;
    }
}
