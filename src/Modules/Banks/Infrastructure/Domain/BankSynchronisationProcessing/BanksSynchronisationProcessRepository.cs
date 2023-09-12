using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Domain.BankSynchronisationProcessing;

public class BanksSynchronisationProcessRepository : IBanksSynchronisationProcessRepository
{
    private readonly BanksContext _banksContext;

    public BanksSynchronisationProcessRepository(BanksContext banksContext)
    {
        _banksContext = banksContext;
    }

    public async Task AddAsync(BanksSynchronisationProcess banksSynchronisationProcess)
    {
        await _banksContext.AddAsync(banksSynchronisationProcess);
    }

    public async Task<BanksSynchronisationProcess> GetByIdAsync(BanksSynchronisationProcessId id)
    {
        var bankSynchronisationProcess = await _banksContext.BankSynchronisationProcesses.SingleOrDefaultAsync(b => b.Id == id);

        if (bankSynchronisationProcess is null)
        {
            throw new NotFoundRepositoryException<BanksSynchronisationProcess>(id.Value);
        }

        return bankSynchronisationProcess;
    }
}
