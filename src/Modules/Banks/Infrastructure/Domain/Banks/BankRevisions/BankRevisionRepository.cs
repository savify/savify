using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Domain.Banks.BankRevisions;

public class BankRevisionRepository : IBankRevisionRepository
{
    private readonly BanksContext _banksContext;

    public BankRevisionRepository(BanksContext banksContext)
    {
        _banksContext = banksContext;
    }

    public async Task AddAsync(BankRevision bankRevision)
    {
        await _banksContext.AddAsync(bankRevision);
    }

    public async Task<BankRevision> GetByIdAsync(BankRevisionId id)
    {
        var bankRevision = await _banksContext.BankRevisions.SingleOrDefaultAsync(b => b.Id == id);

        if (bankRevision is null)
        {
            throw new NotFoundRepositoryException<BankRevision>(id.Value);
        }

        return bankRevision;
    }
}
