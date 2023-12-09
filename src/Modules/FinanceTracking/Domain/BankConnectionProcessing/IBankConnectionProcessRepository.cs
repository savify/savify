using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing;

public interface IBankConnectionProcessRepository
{
    Task AddAsync(BankConnectionProcess bankConnectionProcess);

    Task<BankConnectionProcess> GetByIdAsync(BankConnectionProcessId id);

    Task<BankConnectionProcess> GetByIdAndUserIdAsync(BankConnectionProcessId id, UserId userId);
}
