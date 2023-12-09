using App.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomerRepository : ISaltEdgeCustomerRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public SaltEdgeCustomerRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(SaltEdgeCustomer customer)
    {
        await _financeTrackingContext.AddAsync(customer);
    }

    public async Task<SaltEdgeCustomer> GetAsync(Guid userId)
    {
        var customer = await GetOrDefaultAsync(userId);

        if (customer is null)
        {
            throw new NotFoundRepositoryException<SaltEdgeCustomer>(userId);
        }

        return customer;
    }

    public async Task<SaltEdgeCustomer?> GetOrDefaultAsync(Guid userId)
    {
        return _financeTrackingContext.SaltEdgeCustomers.Local.SingleOrDefault(x => x.Identifier == userId) ??
               await _financeTrackingContext.SaltEdgeCustomers.SingleOrDefaultAsync(x => x.Identifier == userId);
    }
}
