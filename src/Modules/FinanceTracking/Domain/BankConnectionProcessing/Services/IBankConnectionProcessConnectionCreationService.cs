using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessConnectionCreationService
{
    public Task<Result<BankConnection, CreateConnectionError>> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId);
}
