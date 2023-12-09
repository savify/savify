using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessRedirectionService
{
    public Task<Result<Redirection, RedirectionError>> Redirect(BankConnectionProcessId id, UserId userId, BankId bankId);
}
