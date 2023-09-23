using App.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomerRepository : ISaltEdgeCustomerRepository
{
    private readonly WalletsContext _walletsContext;

    public SaltEdgeCustomerRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(SaltEdgeCustomer customer)
    {
        await _walletsContext.AddAsync(customer);
    }

    public async Task<SaltEdgeCustomer> GetAsync(Guid userId)
    {
        var maybeCustomer = await GetOrDefaultAsync(userId);

        if (maybeCustomer is null)
        {
            throw new NotFoundRepositoryException<SaltEdgeCustomer>(userId);
        }

        return maybeCustomer;
    }

    public async Task<SaltEdgeCustomer?> GetOrDefaultAsync(Guid userId)
    {
        return _walletsContext.SaltEdgeCustomers.Local.SingleOrDefault(x => x.Identifier == userId) ??
               await _walletsContext.SaltEdgeCustomers.SingleOrDefaultAsync(x => x.Identifier == userId);
    }
}
