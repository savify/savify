namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;

public class CreateCustomerResponseContent
{
    public required string Id { get; set; }

    public required string Identifier { get; set; }

    public required string Secret { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
