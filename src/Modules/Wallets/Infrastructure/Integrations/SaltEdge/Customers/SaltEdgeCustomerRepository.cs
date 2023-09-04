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
        var localCustomers = _walletsContext.SaltEdgeCustomers.Local;
        if (localCustomers.Any(x => x.Identifier == userId))
        {
            return await Task.FromResult(localCustomers.FirstOrDefault(x => x.Identifier == userId));
        }

        return await _walletsContext.SaltEdgeCustomers.FirstOrDefaultAsync(x => x.Identifier == userId);
    }
}
