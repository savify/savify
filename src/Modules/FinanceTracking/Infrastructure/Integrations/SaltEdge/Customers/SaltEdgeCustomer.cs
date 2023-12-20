namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomer(string id, Guid identifier)
{
    public string Id { get; } = id;

    public Guid Identifier { get; } = identifier;
}
