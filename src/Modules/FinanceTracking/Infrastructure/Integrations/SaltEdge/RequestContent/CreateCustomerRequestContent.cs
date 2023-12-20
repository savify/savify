namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.RequestContent;

public class CreateCustomerRequestContent(string identifier)
{
    public string Identifier { get; } = identifier;
}
