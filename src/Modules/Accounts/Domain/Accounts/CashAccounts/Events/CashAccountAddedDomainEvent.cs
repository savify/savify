using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Accounts.CashAccounts.Events;

public class CashAccountAddedDomainEvent : DomainEventBase
{
    public AccountId AccountId { get; }
    
    public UserId UserId { get; }
    
    public Currency Currency { get; }

    public CashAccountAddedDomainEvent(AccountId accountId, UserId userId, Currency currency)
    {
        AccountId = accountId;
        UserId = userId;
        Currency = currency;
    }
}
