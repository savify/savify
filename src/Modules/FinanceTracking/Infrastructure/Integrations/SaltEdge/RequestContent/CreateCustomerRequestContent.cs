namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.RequestContent;

public class CreateCustomerRequestContent
{
    public string Identifier { get; }

    public CreateCustomerRequestContent(string identifier)
    {
        Identifier = identifier;
    }
}
