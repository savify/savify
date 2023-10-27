using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessInitiationService
{
    public Task InitiateForAsync(UserId userId);
}
