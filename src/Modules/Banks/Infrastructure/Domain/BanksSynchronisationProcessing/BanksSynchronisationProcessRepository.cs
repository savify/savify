using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationProcessRepository(BanksContext banksContext) : IBanksSynchronisationProcessRepository
{
    public async Task AddAsync(BanksSynchronisationProcess banksSynchronisationProcess)
    {
        await banksContext.AddAsync(banksSynchronisationProcess);
    }

    public async Task<BanksSynchronisationProcess> GetByIdAsync(BanksSynchronisationProcessId id)
    {
        var bankSynchronisationProcess = await banksContext.BankSynchronisationProcesses.SingleOrDefaultAsync(b => b.Id == id);

        if (bankSynchronisationProcess is null)
        {
            throw new NotFoundRepositoryException<BanksSynchronisationProcess>(id.Value);
        }

        return bankSynchronisationProcess;
    }
}
