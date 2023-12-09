using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing;

public class BankConnectionProcessRepository : IBankConnectionProcessRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public BankConnectionProcessRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(BankConnectionProcess bankConnectionProcess)
    {
        await _financeTrackingContext.AddAsync(bankConnectionProcess);
    }

    public async Task<BankConnectionProcess> GetByIdAsync(BankConnectionProcessId id)
    {
        var bankConnectionProcess = await _financeTrackingContext.BankConnectionProcesses.SingleOrDefaultAsync(x => x.Id == id);

        if (bankConnectionProcess == null)
        {
            throw new NotFoundRepositoryException<BankConnectionProcess>(id.Value);
        }

        return bankConnectionProcess;
    }

    public async Task<BankConnectionProcess> GetByIdAndUserIdAsync(BankConnectionProcessId id, UserId userId)
    {
        var bankConnectionProcess = await _financeTrackingContext.BankConnectionProcesses.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (bankConnectionProcess == null)
        {
            throw new NotFoundRepositoryException<BankConnectionProcess>(
                "Bank connection process with id '{0}' was not found for user with id '{1}'",
                new object[] { id.Value, userId.Value });
        }

        return bankConnectionProcess;
    }
}
