using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

[method: JsonConstructor]
public class UserRegistrationConfirmedNotification(
    Guid id,
    Guid correlationId,
    UserRegistrationConfirmedDomainEvent domainEvent)
    : DomainEventNotificationBase<UserRegistrationConfirmedDomainEvent>(id, correlationId, domainEvent);
