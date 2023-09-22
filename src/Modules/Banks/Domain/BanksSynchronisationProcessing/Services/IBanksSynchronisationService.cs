namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

public interface IBanksSynchronisationService
{
    public Task SynchroniseAsync(BanksSynchronisationProcessId banksSynchronisationProcessId);

    public Task SynchroniseAsync(BanksSynchronisationProcessId banksSynchronisationProcessId, DateTime fromDate);
}
