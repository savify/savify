using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;

namespace App.Modules.Banks.Domain.Banks.Events;

public class BankUpdatedDomainEvent : DomainEventBase
{
    public BankId BankId { get; }

    public BanksSynchronisationProcessId BanksSynchronisationProcessId { get; }

    public string Name { get; }

    public bool WasDisabled { get; }

    public bool IsRegulated { get; }

    public int? MaxConsentDays { get; }

    public string DefaultLogoUrl { get; }

    public BankUpdatedDomainEvent(
        BankId bankId,
        BanksSynchronisationProcessId banksSynchronisationProcessId,
        string name,
        bool wasDisabled,
        bool isRegulated,
        int? maxConsentDays,
        string defaultLogoUrl)
    {
        BankId = bankId;
        BanksSynchronisationProcessId = banksSynchronisationProcessId;
        Name = name;
        WasDisabled = wasDisabled;
        IsRegulated = isRegulated;
        MaxConsentDays = maxConsentDays;
        DefaultLogoUrl = defaultLogoUrl;
    }
}
