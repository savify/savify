using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Accounts.DebitAccounts.Events
{
    internal class DebitAccountAddedDomainEvent : DomainEventBase
    {
        public AccountId AccountId { get; }

        public UserId UserId { get; }

        public Currency Currency { get; }

        public DebitAccountAddedDomainEvent(AccountId accountId, UserId userId, Currency currency)
        {
            AccountId = accountId;
            UserId = userId;
            Currency = currency;
        }
    }
}
