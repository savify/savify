using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.Banks.BankRevisions.Events;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;

namespace App.Modules.Banks.Domain.Banks.BankRevisions;

public class BankRevision : Entity, IAggregateRoot
{
    public BankRevisionId Id { get; private set; }

    internal BankId BankId { get; private set; }

    internal BanksSynchronisationProcessId BanksSynchronisationProcessId { get; private set; }

    private BankRevisionType _revisionType;

    private string _name;

    private BankStatus _status;

    private bool _isRegulated;

    private int? _maxConsentDays;

    private string? _logoUrl;

    private string _defaultLogoUrl;

    private DateTime _createdAt;

    public static BankRevision CreateNew(
        BankId bankId,
        BanksSynchronisationProcessId banksSynchronisationProcessId,
        BankRevisionType revisionType,
        string name,
        BankStatus status,
        bool isRegulated,
        int? maxConsentDays,
        string? logoUrl,
        string defaultLogoUrl)
    {
        return new BankRevision(
            bankId,
            banksSynchronisationProcessId,
            revisionType,
            name,
            status,
            isRegulated,
            maxConsentDays,
            logoUrl,
            defaultLogoUrl);
    }

    private BankRevision(
        BankId bankId,
        BanksSynchronisationProcessId banksSynchronisationProcessId,
        BankRevisionType revisionType,
        string name,
        BankStatus status,
        bool isRegulated,
        int? maxConsentDays,
        string? logoUrl,
        string defaultLogoUrl)
    {
        Id = new BankRevisionId(Guid.NewGuid());
        BankId = bankId;
        BanksSynchronisationProcessId = banksSynchronisationProcessId;
        _revisionType = revisionType;
        _name = name;
        _status = status;
        _isRegulated = isRegulated;
        _maxConsentDays = maxConsentDays;
        _logoUrl = logoUrl;
        _defaultLogoUrl = defaultLogoUrl;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new BankRevisionCreatedDomainEvent(Id, BankId, _revisionType));
    }

    private BankRevision() { }
}
