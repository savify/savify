namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Accounts;

public class SaltEdgeAccount(string id, string name, string nature, double balance, string currencyCode)
{
    public string Id { get; } = id;

    public string Name { get; } = name;

    public string Nature { get; } = nature;

    public double Balance { get; } = balance;

    public string CurrencyCode { get; } = currencyCode;
}
