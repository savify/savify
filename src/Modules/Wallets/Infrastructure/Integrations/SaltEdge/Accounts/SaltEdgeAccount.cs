namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Accounts;

public class SaltEdgeAccount
{
    public string Id { get; }
    
    public string Name { get; }
    
    public string Nature { get; }
    
    public double Balance { get; }
    
    public string CurrencyCode { get; }

    public SaltEdgeAccount(string id, string name, string nature, double balance, string currencyCode)
    {
        Id = id;
        Name = name;
        Nature = nature;
        Balance = balance;
        CurrencyCode = currencyCode;
    }
}
