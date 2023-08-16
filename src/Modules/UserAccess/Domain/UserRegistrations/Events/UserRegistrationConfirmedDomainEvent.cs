using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Events;

public class UserRegistrationConfirmedDomainEvent : DomainEventBase
{
    public UserRegistrationId UserRegistrationId { get; }

    public string Email { get; }

    public string Name { get; }

    public Language PreferredLanguage { get; }

    public UserRegistrationConfirmedDomainEvent(UserRegistrationId userRegistrationId, string email, string name, Language preferredLanguage)
    {
        UserRegistrationId = userRegistrationId;
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
    }
}
