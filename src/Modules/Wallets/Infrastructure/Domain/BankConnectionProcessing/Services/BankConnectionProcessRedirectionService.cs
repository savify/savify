using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessRedirectionService : IBankConnectionProcessRedirectionService
{
    public Task<Redirection> Redirect(UserId userId, BankId bankId)
    {
        throw new NotImplementedException();
    }
}
