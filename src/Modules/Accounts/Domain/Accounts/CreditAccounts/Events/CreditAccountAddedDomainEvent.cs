using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Accounts.CreditAccounts.Events;

public class CreditAccountAddedDomainEvent : DomainEventBase
{
    public AccountId AccountId { get; }

    public UserId UserId { get; }

    public Currency Currency { get; }

    public CreditAccountAddedDomainEvent(AccountId accountId, UserId userId, Currency currency)
    {
        AccountId = accountId;
        UserId = userId;
        Currency = currency;
    }
}
