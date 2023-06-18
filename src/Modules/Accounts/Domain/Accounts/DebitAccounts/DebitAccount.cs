using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts.Events;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Accounts.DebitAccounts
{
    public class DebitAccount : Entity, IAggregateRoot
    {
        public AccountId Id { get; private set; }

        internal UserId UserId { get; private set; }

        private string _title;

        private Currency _currency;

        private int _balance;

        private DateTime _createdAt;

        public static DebitAccount AddNew(UserId userId, string title, Currency currency, int balance = 0)
        {
            return new DebitAccount(userId, title, currency, balance);
        }

        private DebitAccount(UserId userId, string title, Currency currency, int balance)
        {
            Id = new AccountId(Guid.NewGuid());
            UserId = userId;
            _title = title;
            _currency = currency;
            _balance = balance;
            _createdAt = DateTime.UtcNow;

            AddDomainEvent(new DebitAccountAddedDomainEvent(Id, userId, _currency));
        }
    }
}
