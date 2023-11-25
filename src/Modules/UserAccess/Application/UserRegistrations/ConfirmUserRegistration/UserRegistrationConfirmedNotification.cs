using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class UserRegistrationConfirmedNotification : DomainEventNotificationBase<UserRegistrationConfirmedDomainEvent>
{
    [JsonConstructor]
    public UserRegistrationConfirmedNotification(Guid id, Guid correlationId, UserRegistrationConfirmedDomainEvent domainEvent) : base(id, correlationId, domainEvent)
    {
    }
}
