using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.Users.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

public class UserCreatedNotification : DomainEventNotificationBase<UserCreatedDomainEvent>
{
    [JsonConstructor]
    public UserCreatedNotification(Guid id, Guid correlationId, UserCreatedDomainEvent domainEvent) : base(id, correlationId, domainEvent)
    {
    }
}
