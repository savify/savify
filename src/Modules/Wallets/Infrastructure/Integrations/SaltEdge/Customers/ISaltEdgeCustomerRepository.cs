namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

public interface ISaltEdgeCustomerRepository
{
    public Task AddAsync(SaltEdgeCustomer customer);

    public Task<SaltEdgeCustomer?> GetSaltEdgeCustomerForAsync(Guid userId);
}
