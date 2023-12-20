using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.BankConnections;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnections;

public class BankConnectionRepository(FinanceTrackingContext financeTrackingContext) : IBankConnectionRepository
{
    public async Task AddAsync(BankConnection bankConnection)
    {
        await financeTrackingContext.AddAsync(bankConnection);
    }

    public async Task<BankConnection> GetByIdAsync(BankConnectionId id)
    {
        var bankConnection = financeTrackingContext.BankConnections.Local.SingleOrDefault(x => x.Id == id) ??
                             await financeTrackingContext.BankConnections.SingleOrDefaultAsync(x => x.Id == id);

        if (bankConnection == null)
        {
            throw new NotFoundRepositoryException<BankConnection>(id.Value);
        }

        return bankConnection;
    }

    public void Remove(BankConnection bankConnection)
    {
        financeTrackingContext.Remove(bankConnection);
    }
}
