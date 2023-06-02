using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Events;

public class UserRegistrationConfirmedDomainEvent : DomainEventBase
{
    public UserRegistrationId UserRegistrationId { get; }
    
    public string Email { get; }

    public UserRegistrationConfirmedDomainEvent(UserRegistrationId userRegistrationId, string email)
    {
        UserRegistrationId = userRegistrationId;
        Email = email;
    }
}
