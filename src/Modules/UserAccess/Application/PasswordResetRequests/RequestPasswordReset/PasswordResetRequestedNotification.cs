using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

[method: JsonConstructor]
public class PasswordResetRequestedNotification(
    Guid id,
    Guid correlationId,
    PasswordResetRequestedDomainEvent domainEvent)
    : DomainEventNotificationBase<PasswordResetRequestedDomainEvent>(id, correlationId, domainEvent);
