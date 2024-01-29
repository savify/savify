using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Events;

public class UserRegistrationConfirmedDomainEvent(
    UserRegistrationId userRegistrationId,
    string email,
    string name,
    Country country,
    Language preferredLanguage)
    : DomainEventBase
{
    public UserRegistrationId UserRegistrationId { get; } = userRegistrationId;

    public string Email { get; } = email;

    public string Name { get; } = name;

    public Country Country { get; } = country;

    public Language PreferredLanguage { get; } = preferredLanguage;
}
