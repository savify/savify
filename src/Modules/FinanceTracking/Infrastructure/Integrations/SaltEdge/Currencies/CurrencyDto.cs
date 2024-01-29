namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Currencies;

public class CurrencyDto(string code, string name)
{
    public string Code { get; } = code;

    public string Name { get; } = name;
}
