using App.BuildingBlocks.Domain.Results;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

public interface IBanksSynchronisationService
{
    public Task<Result> SynchroniseAsync(BanksSynchronisationProcessId banksSynchronisationProcessId);
}
