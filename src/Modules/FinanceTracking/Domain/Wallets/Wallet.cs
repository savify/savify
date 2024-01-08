using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;

namespace App.Modules.FinanceTracking.Domain.Wallets;

public abstract class Wallet : Entity
{
    public WalletId Id { get; protected set; }

    private List<ManualBalanceChange> _manualBalanceChanges = new();

    public void Load(IEnumerable<IDomainEvent> history)
    {
        foreach (var domainEvent in history)
        {
            Apply(domainEvent);
        }
    }

    public abstract void IncreaseBalance(Money amount);

    public abstract void DecreaseBalance(Money amount);

    protected abstract void Apply(IDomainEvent @event);

    protected void AddManualBalanceChange(Money amount, ManualBalanceChangeType type)
    {
        _manualBalanceChanges.Add(new ManualBalanceChange(type, amount, DateTime.UtcNow));
    }
}
