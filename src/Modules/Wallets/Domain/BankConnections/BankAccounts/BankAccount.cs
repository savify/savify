using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Finance;

namespace App.Modules.Wallets.Domain.BankConnections.BankAccounts;

public class BankAccount : Entity
{
    internal BankAccountId Id { get; private set; }

    internal BankConnectionId BankConnectionId { get; private set; }

    // TODO: rename amount to balance
    internal int Amount { get; }

    internal Currency Currency { get; }

    private string _externalId;

    private string _name;

    internal static BankAccount CreateNew(BankConnectionId bankConnectionId, string externalId, string name, int amount, Currency currency)
    {
        return new BankAccount(bankConnectionId, externalId, name, amount, currency);
    }

    private BankAccount(BankConnectionId bankConnectionId, string externalId, string name, int amount, Currency currency)
    {
        Id = new BankAccountId(Guid.NewGuid());
        BankConnectionId = bankConnectionId;
        _externalId = externalId;
        _name = name;
        Amount = amount;
        Currency = currency;
    }

    private BankAccount() { }
}
