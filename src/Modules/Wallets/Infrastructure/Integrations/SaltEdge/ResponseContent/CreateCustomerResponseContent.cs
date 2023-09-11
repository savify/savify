namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

public class CreateCustomerResponseContent
{
    public string Id { get; set; }

    public string Identifier { get; set; }

    public string Secret { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
