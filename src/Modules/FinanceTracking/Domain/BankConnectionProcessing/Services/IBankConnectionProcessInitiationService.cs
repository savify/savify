using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;

public interface IBankConnectionProcessInitiationService
{
    public Task InitiateForAsync(UserId userId);
}
