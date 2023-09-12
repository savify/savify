using App.Modules.Banks.Domain.BankSynchronisationProcessing;
using App.Modules.Banks.Domain.BankSynchronisationProcessing.Services;

namespace App.Modules.Banks.Infrastructure.Domain.BankSynchronisationProcessing;

public class BankSynchronisationService : IBankSynchronisationService
{
    public Task Synchronise(BankSynchronisationProcessId bankSynchronisationProcessId)
    {
        throw new NotImplementedException();
    }
}
