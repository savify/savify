using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnections;

public class BankConnection : Entity, IAggregateRoot
{
    public BankConnectionId Id { get; private set; }

    private Country _country;

    private BankId _bankId;

    private UserId _userId;

    private Consent _consent;

    private DateTime _createdAt;

    private DateTime? _refreshedAt = null;

    public static BankConnection CreateNew(Country country, BankId bankId, UserId userId, Consent consent)
    {
        return new BankConnection(country, bankId, userId, consent);
    }

    private BankConnection(Country country, BankId bankId, UserId userId, Consent consent)
    {
        Id = new BankConnectionId(Guid.NewGuid());
        _country = country;
        _bankId = bankId;
        _userId = userId;
        _consent = consent;
        _createdAt = DateTime.UtcNow;
    }

    private BankConnection() { }
}
