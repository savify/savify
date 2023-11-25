using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

public class NewUserRegisteredNotification : DomainEventNotificationBase<NewUserRegisteredDomainEvent>
{
    [JsonConstructor]
    public NewUserRegisteredNotification(Guid id, Guid correlationId, NewUserRegisteredDomainEvent domainEvent) : base(id, correlationId, domainEvent)
    {
    }
}
