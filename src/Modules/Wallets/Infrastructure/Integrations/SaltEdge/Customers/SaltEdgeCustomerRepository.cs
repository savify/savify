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

    public async Task<SaltEdgeCustomer?> GetSaltEdgeCustomerForAsync(Guid userId)
    {
        return _walletsContext.SaltEdgeCustomers.Local.FirstOrDefault(x => x.Identifier == userId) ??
               await _walletsContext.SaltEdgeCustomers.FirstOrDefaultAsync(x => x.Identifier == userId);
    }
}
