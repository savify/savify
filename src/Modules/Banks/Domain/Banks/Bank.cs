using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.Banks.Events;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.ExternalProviders;

namespace App.Modules.Banks.Domain.Banks;

public class Bank : Entity, IAggregateRoot
{
    public BankId Id { get; private set; }

    public string ExternalId { get; private set; }

    public BanksSynchronisationProcessId LastBanksSynchronisationProcessId { get; private set; }

    private ExternalProviderName _externalProviderName;

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
        BanksSynchronisationProcessId banksSynchronisationProcessId,
        ExternalProviderName externalProviderName,
        string externalId,
        string name,
        Country country,
        BankStatus status,
        bool isRegulated,
        int? maxConsentDays,
        string defaultLogoUrl)
    {
        return new Bank(banksSynchronisationProcessId, externalProviderName, externalId, name, country, status, isRegulated, maxConsentDays, defaultLogoUrl);
    }

    public void Update(BanksSynchronisationProcessId banksSynchronisationProcessId, string name, bool wasDisabled, bool isRegulated, int? maxConsentDays, string defaultLogoUrl)
    {
        // TODO: maybe we should add some logic to save data that was updated on bank?
        LastBanksSynchronisationProcessId = banksSynchronisationProcessId;
        _name = name;
        _isRegulated = isRegulated;
        _maxConsentDays = maxConsentDays;
        _defaultLogoUrl = defaultLogoUrl;
        _updatedAt = DateTime.UtcNow;

        if (wasDisabled)
        {
            _status = BankStatus.Disabled;
        }

        AddDomainEvent(new BankUpdatedDomainEvent(
            Id,
            banksSynchronisationProcessId,
            name,
            wasDisabled,
            isRegulated,
            maxConsentDays,
            defaultLogoUrl));
    }

    public void Disable() => _status = BankStatus.Disabled;

    public bool IsFake() => _country.IsFake();

    public bool IsEnabled() => _status == BankStatus.Enabled || _status == BankStatus.Beta;

    public bool IsInBeta() => _status == BankStatus.Beta;

    public Bank(
        BanksSynchronisationProcessId banksSynchronisationProcessId,
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
        LastBanksSynchronisationProcessId = banksSynchronisationProcessId;
        ExternalId = externalId;
        _externalProviderName = externalProviderName;
        _name = name;
        _country = country;
        _status = status;
        _isRegulated = isRegulated;
        _maxConsentDays = maxConsentDays;
        _defaultLogoUrl = defaultLogoUrl;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new BankAddedDomainEvent(Id, _externalProviderName, ExternalId));
    }

    private Bank() { }
}
