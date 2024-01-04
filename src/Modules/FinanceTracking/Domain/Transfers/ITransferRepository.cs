namespace App.Modules.FinanceTracking.Domain.Transfers;
public interface ITransferRepository
{
    Task AddAsync(Transfer transfer);

    void Remove(Transfer transfer);

    Task<Transfer> GetByIdAsync(TransferId id);
}
