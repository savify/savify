using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Events;

public class UserRegistrationExpiredDomainEvent : DomainEventBase
{
    public UserRegistrationId UserRegistrationId { get; }

    public UserRegistrationExpiredDomainEvent(UserRegistrationId userRegistrationId)
    {
        UserRegistrationId = userRegistrationId;
    }
}
