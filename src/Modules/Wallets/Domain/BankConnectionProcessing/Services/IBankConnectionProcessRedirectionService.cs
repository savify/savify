using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessRedirectionService
{
    public Task<Redirection> Redirect(BankConnectionProcessId id, UserId userId, BankId bankId);
}
