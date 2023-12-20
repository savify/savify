using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

[method: JsonConstructor]
public class UserRegistrationRenewedNotification(
    Guid id,
    Guid correlationId,
    UserRegistrationRenewedDomainEvent domainEvent)
    : DomainEventNotificationBase<UserRegistrationRenewedDomainEvent>(id, correlationId, domainEvent);
