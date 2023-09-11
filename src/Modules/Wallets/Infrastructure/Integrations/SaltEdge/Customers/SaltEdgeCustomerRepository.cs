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

    public async Task<SaltEdgeCustomer?> GetSaltEdgeCustomerOrDefaultAsync(Guid userId)
    {
        return _walletsContext.SaltEdgeCustomers.Local.SingleOrDefault(x => x.Identifier == userId) ??
               await _walletsContext.SaltEdgeCustomers.SingleOrDefaultAsync(x => x.Identifier == userId);
    }
}
