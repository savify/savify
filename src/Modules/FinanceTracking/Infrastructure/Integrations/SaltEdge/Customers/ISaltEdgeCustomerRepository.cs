namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

public interface ISaltEdgeCustomerRepository
{
    public Task AddAsync(SaltEdgeCustomer customer);

    public Task<SaltEdgeCustomer> GetAsync(Guid userId);

    public Task<SaltEdgeCustomer?> GetOrDefaultAsync(Guid userId);
}
