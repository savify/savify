using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing;

public class BankConnectionProcessRepository(FinanceTrackingContext financeTrackingContext)
    : IBankConnectionProcessRepository
{
    public async Task AddAsync(BankConnectionProcess bankConnectionProcess)
    {
        await financeTrackingContext.AddAsync(bankConnectionProcess);
    }

    public async Task<BankConnectionProcess> GetByIdAsync(BankConnectionProcessId id)
    {
        var bankConnectionProcess = await financeTrackingContext.BankConnectionProcesses.SingleOrDefaultAsync(x => x.Id == id);

        if (bankConnectionProcess == null)
        {
            throw new NotFoundRepositoryException<BankConnectionProcess>(id.Value);
        }

        return bankConnectionProcess;
    }

    public async Task<BankConnectionProcess> GetByIdAndUserIdAsync(BankConnectionProcessId id, UserId userId)
    {
        var bankConnectionProcess = await GetByIdAsync(id);

        if (bankConnectionProcess.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return bankConnectionProcess;
    }
}
