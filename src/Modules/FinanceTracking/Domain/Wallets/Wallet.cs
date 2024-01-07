using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets;

public abstract class Wallet : Entity
{
    public WalletId Id { get; protected set; }

    public void Load(IEnumerable<IDomainEvent> history)
    {
        foreach (var domainEvent in history)
        {
            Apply(domainEvent);
        }
    }

    protected abstract void Apply(IDomainEvent @event);
}
