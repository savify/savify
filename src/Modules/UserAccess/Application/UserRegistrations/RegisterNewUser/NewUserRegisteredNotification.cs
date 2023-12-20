using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

[method: JsonConstructor]
public class NewUserRegisteredNotification(Guid id, Guid correlationId, NewUserRegisteredDomainEvent domainEvent)
    : DomainEventNotificationBase<NewUserRegisteredDomainEvent>(id, correlationId, domainEvent);
