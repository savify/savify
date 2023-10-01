using App.BuildingBlocks.Domain.Results;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessConnectionCreationService
{
    public Task<Result<BankConnection, CreateConnectionError>> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId);
}
