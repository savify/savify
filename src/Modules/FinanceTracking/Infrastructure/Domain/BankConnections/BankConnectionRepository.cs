using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.BankConnections;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnections;

public class BankConnectionRepository : IBankConnectionRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public BankConnectionRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(BankConnection bankConnection)
    {
        await _financeTrackingContext.AddAsync(bankConnection);
    }

    public async Task<BankConnection> GetByIdAsync(BankConnectionId id)
    {
        var bankConnection = _financeTrackingContext.BankConnections.Local.SingleOrDefault(x => x.Id == id) ??
                             await _financeTrackingContext.BankConnections.SingleOrDefaultAsync(x => x.Id == id);

        if (bankConnection == null)
        {
            throw new NotFoundRepositoryException<BankConnection>(id.Value);
        }

        return bankConnection;
    }

    public void Remove(BankConnection bankConnection)
    {
        _financeTrackingContext.Remove(bankConnection);
    }
}
