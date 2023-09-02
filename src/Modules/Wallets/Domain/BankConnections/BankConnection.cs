using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnections;

public class BankConnection : Entity, IAggregateRoot
{
    public BankConnectionId Id { get; private set; }

    private BankId _bankId;

    private UserId _userId;

    private Consent _consent;

    private List<BankAccount> _accounts;

    private DateTime _createdAt;

    private DateTime? _refreshedAt = null;

    public static BankConnection CreateFromBankConnectionProcess(BankConnectionProcessId id, BankId bankId, UserId userId, Consent consent, List<BankAccount> accounts)
    {
        return new BankConnection(id.Value, bankId, userId, consent, accounts);
    }

    public bool HasMultipleBankAccounts() => _accounts.Count > 1;

    private BankConnection(Guid id, BankId bankId, UserId userId, Consent consent, List<BankAccount> accounts)
    {
        Id = new BankConnectionId(id);
        _bankId = bankId;
        _userId = userId;
        _consent = consent;
        _accounts = accounts;
        _createdAt = DateTime.UtcNow;
    }

    private BankConnection() { }
}
