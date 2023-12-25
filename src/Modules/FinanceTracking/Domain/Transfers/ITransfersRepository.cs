namespace App.Modules.FinanceTracking.Domain.Transfers;
public interface ITransfersRepository
{
    Task AddAsync(Transfer transfer);
    void Remove(Transfer transfer);
}
