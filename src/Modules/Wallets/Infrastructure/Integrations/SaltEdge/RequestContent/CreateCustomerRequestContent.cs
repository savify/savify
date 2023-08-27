namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.RequestContent;

public class CreateCustomerRequestContent
{
    public string Identifier { get; }

    public CreateCustomerRequestContent(string identifier)
    {
        Identifier = identifier;
    }
}
