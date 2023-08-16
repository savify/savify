using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Events;

public class NewUserRegisteredDomainEvent : DomainEventBase
{
    public UserRegistrationId UserRegistrationId { get; }

    public string Email { get; }

    public string Name { get; }

    public Country Country { get; }

    public Language PreferredLanguage { get; }

    public ConfirmationCode ConfirmationCode { get; }

    public NewUserRegisteredDomainEvent(
        UserRegistrationId userRegistrationId,
        string email,
        string name,
        Country country,
        Language preferredLanguage,
        ConfirmationCode confirmationCode)
    {
        UserRegistrationId = userRegistrationId;
        Email = email;
        Name = name;
        Country = country;
        PreferredLanguage = preferredLanguage;
        ConfirmationCode = confirmationCode;
    }
}
