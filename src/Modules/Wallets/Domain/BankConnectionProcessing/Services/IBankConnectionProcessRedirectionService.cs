using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessRedirectionService
{
    public Task<Result<Redirection, RedirectionError>> Redirect(BankConnectionProcessId id, UserId userId, BankId bankId);
}
