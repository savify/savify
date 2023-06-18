using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.CashAccounts;

public class CashAccountRepository : ICashAccountRepository
{
    private readonly AccountsContext _accountsContext;

    public CashAccountRepository(AccountsContext accountsContext)
    {
        _accountsContext = accountsContext;
    }

    public async Task AddAsync(CashAccount account)
    {
        await _accountsContext.AddAsync(account);
    }

    public async Task<CashAccount> GetByIdAsync(AccountId id)
    {
        var account = await _accountsContext.CashAccounts.FirstOrDefaultAsync(x => x.Id == id);

        if (account == null)
        {
            throw new NotFoundRepositoryException<CashAccount>(id.Value);
        }

        return account;
    }
}
