using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.BankConnections.Events;
using App.Modules.FinanceTracking.Domain.BankConnections.Rules;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.BankConnections;

public class BankConnection : Entity, IAggregateRoot
{
    public BankConnectionId Id { get; private set; }

    private BankId _bankId;

    private UserId _userId;

    private Consent _consent;

    private List<BankAccount> _accounts = new();

    private DateTime _createdAt;

    private DateTime? _refreshedAt = null;

    public static BankConnection CreateFromBankConnectionProcess(BankConnectionProcessId id, BankId bankId, UserId userId, Consent consent)
    {
        return new BankConnection(id.Value, bankId, userId, consent);
    }

    public void AddBankAccount(string externalId, string name, int balance, Currency currency)
    {
        _accounts.Add(BankAccount.CreateNew(Id, externalId, name, balance, currency));
    }

    public BankAccount GetBankAccountById(BankAccountId bankAccountId)
    {
        CheckRules(new BankConnectionShouldHaveBankAccountWithGivenIdRule(_accounts, bankAccountId));

        return _accounts.Single(a => a.Id == bankAccountId);
    }

    public BankAccount GetSingleBankAccount()
    {
        CheckRules(new BankConnectionMustHaveOnlyOneAccountRule(_accounts));

        return _accounts.Single();
    }

    public bool HasMultipleBankAccounts() => _accounts.Count > 1;

    private BankConnection(Guid id, BankId bankId, UserId userId, Consent consent)
    {
        Id = new BankConnectionId(id);
        _bankId = bankId;
        _userId = userId;
        _consent = consent;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new BankConnectionCreatedDomainEvent(Id, _bankId, _userId));
    }

    private BankConnection() { }
}
