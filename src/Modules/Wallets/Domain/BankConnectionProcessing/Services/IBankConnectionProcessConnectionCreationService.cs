using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessConnectionCreationService
{
    public Task<BankConnection> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId);
}
