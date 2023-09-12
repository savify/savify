namespace App.Modules.Banks.Domain.BankSynchronisationProcessing.Services;

public interface IBankSynchronisationService
{
    public Task Synchronise(BankSynchronisationProcessId bankSynchronisationProcessId);
}
