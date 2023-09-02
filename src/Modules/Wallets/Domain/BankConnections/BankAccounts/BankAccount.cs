using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnections.BankAccounts;

public class BankAccount : Entity
{
    internal BankAccountId Id { get; private set; }

    internal BankConnectionId BankConnectionId { get; private set; }

    private string _externalId;

    private string _name;

    private int _amount;

    private Currency _currency;

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
        _amount = amount;
        _currency = currency;
    }

    private BankAccount() { }
}
