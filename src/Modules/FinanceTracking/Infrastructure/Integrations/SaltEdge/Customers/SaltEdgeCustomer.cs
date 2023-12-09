namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomer
{
    public string Id { get; }

    public Guid Identifier { get; }

    public SaltEdgeCustomer(string id, Guid identifier)
    {
        Id = id;
        Identifier = identifier;
    }
}
