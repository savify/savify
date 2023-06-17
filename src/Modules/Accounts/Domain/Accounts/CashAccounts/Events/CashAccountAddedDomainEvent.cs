using App.BuildingBlocks.Domain;

namespace App.Modules.Accounts.Domain.Accounts.CashAccounts.Events;

public class CashAccountAddedDomainEvent : DomainEventBase
{
    public AccountId AccountId { get; }
    
    public Currency Currency { get; }

    public CashAccountAddedDomainEvent(AccountId accountId, Currency currency)
    {
        AccountId = accountId;
        Currency = currency;
    }
}
