using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public interface ITransferRepository
{
    Task AddAsync(Transfer transfer);

    void Remove(Transfer transfer);

    Task<Transfer> GetByIdAsync(TransferId id);

    Task<Transfer> GetByIdAndUserIdAsync(TransferId id, UserId userId);
}
