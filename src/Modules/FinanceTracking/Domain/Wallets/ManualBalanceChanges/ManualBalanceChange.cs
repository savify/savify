using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;

public record ManualBalanceChange
{
    public ManualBalanceChangeType Type { get; init; }

    public Money Amount { get; init; }

    public DateTime MadeOn { get; init; }

    public ManualBalanceChange(ManualBalanceChangeType type, Money amount, DateTime madeOn)
    {
        Type = type;
        Amount = amount;
        MadeOn = madeOn;
    }

    private ManualBalanceChange() { }
}
