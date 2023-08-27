using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomerRepository
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
        return await _walletsContext.SaltEdgeCustomers.FirstOrDefaultAsync(x => x.Identifier == userId);
    }
}
