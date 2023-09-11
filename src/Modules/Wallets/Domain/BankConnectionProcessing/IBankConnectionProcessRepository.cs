using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public interface IBankConnectionProcessRepository
{
    Task AddAsync(BankConnectionProcess bankConnectionProcess);

    Task<BankConnectionProcess> GetByIdAsync(BankConnectionProcessId id);

    Task<BankConnectionProcess> GetByIdAndUserIdAsync(BankConnectionProcessId id, UserId userId);
}
