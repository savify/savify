using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.Banks.Events;
using App.Modules.Banks.Domain.ExternalProviders;

namespace App.Modules.Banks.Domain.Banks;

public class Bank : Entity, IAggregateRoot
{
    public BankId Id { get; private set; }

    private ExternalProviderName _externalProviderName;

    private string _externalId;

    private string _name;

    private Country _country;

    private BankStatus _status;

    private bool _isRegulated;

    private int? _maxConsentDays;

    private string? _logoUrl;

    private string _defaultLogoUrl;

    private DateTime _createdAt;

    private DateTime? _updatedAt;

    public static Bank AddNew(
        ExternalProviderName externalProviderName,
        string externalId,
        string name,
        Country country,
        BankStatus status,
        bool isRegulated,
        int? maxConsentDays,
        string defaultLogoUrl)
    {
        return new Bank(externalProviderName, externalId, name, country, status, isRegulated, maxConsentDays, defaultLogoUrl);
    }

    public bool IsFake() => _country.IsFake();

    public bool IsEnabled() => _status == BankStatus.Enabled || _status == BankStatus.Beta;

    public bool IsInBeta() => _status == BankStatus.Beta;

    public Bank(
        ExternalProviderName externalProviderName,
        string externalId,
        string name,
        Country country,
        BankStatus status,
        bool isRegulated,
        int? maxConsentDays,
        string defaultLogoUrl)
    {
        Id = new BankId(Guid.NewGuid());
        _externalProviderName = externalProviderName;
        _externalId = externalId;
        _name = name;
        _country = country;
        _status = status;
        _isRegulated = isRegulated;
        _maxConsentDays = maxConsentDays;
        _defaultLogoUrl = defaultLogoUrl;

        AddDomainEvent(new BankAddedDomainEvent(Id, externalProviderName, externalId));
    }

    private Bank() { }
}
