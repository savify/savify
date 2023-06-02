using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Events;

public class UserRegistrationRenewedDomainEvent : DomainEventBase
{
    public UserRegistrationId UserRegistrationId { get; }

    public string Email { get; }
    
    public string Name { get; }
    
    public Language PreferredLanguage { get; }

    public ConfirmationCode ConfirmationCode { get; }

    public UserRegistrationRenewedDomainEvent(
        UserRegistrationId userRegistrationId,
        string email,
        string name,
        Language preferredLanguage,
        ConfirmationCode confirmationCode)
    {
        UserRegistrationId = userRegistrationId;
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
        ConfirmationCode = confirmationCode;
    }
}
