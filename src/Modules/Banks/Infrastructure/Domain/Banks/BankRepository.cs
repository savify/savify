using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Domain.Banks;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Domain.Banks;

public class BankRepository : IBankRepository
{
    private readonly BanksContext _banksContext;

    public BankRepository(BanksContext banksContext)
    {
        _banksContext = banksContext;
    }

    public async Task AddAsync(Bank bank)
    {
        await _banksContext.AddAsync(bank);
    }

    public async Task<Bank> GetByIdAsync(BankId id)
    {
        var bank = await _banksContext.Banks.SingleOrDefaultAsync(b => b.Id == id);

        if (bank is null)
        {
            throw new NotFoundRepositoryException<Bank>(id.Value);
        }

        return bank;
    }
}
