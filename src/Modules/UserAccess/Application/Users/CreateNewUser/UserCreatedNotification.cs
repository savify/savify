using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.Users.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

[method: JsonConstructor]
public class UserCreatedNotification(Guid id, Guid correlationId, UserCreatedDomainEvent domainEvent)
    : DomainEventNotificationBase<UserCreatedDomainEvent>(id, correlationId, domainEvent);
