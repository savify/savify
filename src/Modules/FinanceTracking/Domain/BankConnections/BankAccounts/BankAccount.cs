using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

public class BankAccount : Entity
{
    internal BankAccountId Id { get; private set; }

    internal BankConnectionId BankConnectionId { get; private set; }

    internal int Balance { get; }

    internal Currency Currency { get; }

    private string _externalId;

    private string _name;

    internal static BankAccount CreateNew(BankConnectionId bankConnectionId, string externalId, string name, int balance, Currency currency)
    {
        return new BankAccount(bankConnectionId, externalId, name, balance, currency);
    }

    private BankAccount(BankConnectionId bankConnectionId, string externalId, string name, int balance, Currency currency)
    {
        Id = new BankAccountId(Guid.NewGuid());
        BankConnectionId = bankConnectionId;
        _externalId = externalId;
        _name = name;
        Balance = balance;
        Currency = currency;
    }

    private BankAccount() { }
}
