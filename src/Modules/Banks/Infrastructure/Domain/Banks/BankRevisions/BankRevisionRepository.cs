using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Domain.Banks.BankRevisions;

public class BankRevisionRepository(BanksContext banksContext) : IBankRevisionRepository
{
    public async Task AddAsync(BankRevision bankRevision)
    {
        await banksContext.AddAsync(bankRevision);
    }

    public async Task<BankRevision> GetByIdAsync(BankRevisionId id)
    {
        var bankRevision = await banksContext.BankRevisions.SingleOrDefaultAsync(b => b.Id == id);

        if (bankRevision is null)
        {
            throw new NotFoundRepositoryException<BankRevision>(id.Value);
        }

        return bankRevision;
    }
}
