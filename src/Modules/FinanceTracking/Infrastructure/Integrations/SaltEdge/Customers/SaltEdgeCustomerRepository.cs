using App.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomerRepository(FinanceTrackingContext financeTrackingContext) : ISaltEdgeCustomerRepository
{
    public async Task AddAsync(SaltEdgeCustomer customer)
    {
        await financeTrackingContext.AddAsync(customer);
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
        return financeTrackingContext.SaltEdgeCustomers.Local.SingleOrDefault(x => x.Identifier == userId) ??
               await financeTrackingContext.SaltEdgeCustomers.SingleOrDefaultAsync(x => x.Identifier == userId);
    }
}
