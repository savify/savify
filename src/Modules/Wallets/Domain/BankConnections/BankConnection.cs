using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.BankConnections.Rules;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnections;

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

    public void AddBankAccount(string externalId, string name, int amount, Currency currency)
    {
        _accounts.Add(BankAccount.CreateNew(Id, externalId, name, amount, currency));
    }

    public BankAccount GetBankAccountById(BankAccountId bankAccountId)
    {
        CheckRules(new BankConnectionShouldHaveBankAccountWithGivenIdRule(_accounts, bankAccountId));

        return _accounts.First(a => a.Id == bankAccountId);
    }

    public BankAccount GetSingleBankAccount()
    {
        CheckRules(new BankConnectionMustNotHaveMultipleAccountsRule(HasMultipleBankAccounts()));

        return _accounts.First();
    }

    public bool HasMultipleBankAccounts() => _accounts.Count > 1;

    private BankConnection(Guid id, BankId bankId, UserId userId, Consent consent)
    {
        Id = new BankConnectionId(id);
        _bankId = bankId;
        _userId = userId;
        _consent = consent;
        _createdAt = DateTime.UtcNow;
    }

    private BankConnection() { }
}
