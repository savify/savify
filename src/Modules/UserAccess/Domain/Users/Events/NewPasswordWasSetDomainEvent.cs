using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.Users.Events;

public class NewPasswordWasSetDomainEvent : DomainEventBase
{
    public UserId UserId { get; }

    public string Email { get; }

    public string Name { get; }

    public Language PreferredLanguage { get; }

    public NewPasswordWasSetDomainEvent(
        UserId userId,
        string email,
        string name,
        Language preferredLanguage)
    {
        UserId = userId;
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
    }
}
