namespace App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;

public record ManualBalanceChangeType
{
    public string Value { get; }

    public static ManualBalanceChangeType Increase => new(nameof(Increase));

    public static ManualBalanceChangeType Decrease => new(nameof(Decrease));

    private ManualBalanceChangeType(string value)
    {
        Value = value;
    }
}
