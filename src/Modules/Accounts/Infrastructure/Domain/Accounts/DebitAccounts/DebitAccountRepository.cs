using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.DebitAccounts;

internal class DebitAccountRepository : IDebitAccountRepository
{
    private readonly AccountsContext _accountsContext;

    public DebitAccountRepository(AccountsContext accountsContext)
    {
        _accountsContext = accountsContext;
    }

    public async Task AddAsync(DebitAccount account)
    {
        await _accountsContext.AddAsync(account);
    }

    public async Task<DebitAccount> GetByIdAsync(AccountId id)
    {
        var account = await _accountsContext.DebitAccounts.FirstOrDefaultAsync(account => account.Id == id);

        if (account is null)
        {
            throw new NotFoundRepositoryException<DebitAccount>(id.Value);
        }

        return account;
    }
}