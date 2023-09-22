namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public interface IBanksSynchronisationProcessRepository
{
    public Task AddAsync(BanksSynchronisationProcess banksSynchronisationProcess);

    public Task<BanksSynchronisationProcess> GetByIdAsync(BanksSynchronisationProcessId id);
}
