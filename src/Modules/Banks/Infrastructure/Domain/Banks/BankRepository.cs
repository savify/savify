using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Domain.Banks;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Domain.Banks;

public class BankRepository(BanksContext banksContext) : IBankRepository
{
    public async Task AddAsync(Bank bank)
    {
        await banksContext.AddAsync(bank);
    }

    public async Task<Bank> GetByIdAsync(BankId id)
    {
        var bank = banksContext.Banks.Local.SingleOrDefault(b => b.Id == id) ??
                   await banksContext.Banks.SingleOrDefaultAsync(b => b.Id == id);

        if (bank is null)
        {
            throw new NotFoundRepositoryException<Bank>(id.Value);
        }

        return bank;
    }

    public async Task<Bank?> GetByExternalIdAsync(string externalId)
    {
        return await banksContext.Banks.SingleOrDefaultAsync(b => b.ExternalId == externalId);
    }

    public async Task<List<Bank>> GetAllAsync()
    {
        return await banksContext.Banks.ToListAsync();
    }
}
