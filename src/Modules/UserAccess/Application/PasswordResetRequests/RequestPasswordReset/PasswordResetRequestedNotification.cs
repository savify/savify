using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

public class PasswordResetRequestedNotification : DomainEventNotificationBase<PasswordResetRequestedDomainEvent>
{
    [JsonConstructor]
    public PasswordResetRequestedNotification(Guid id, PasswordResetRequestedDomainEvent domainEvent) : base(id, domainEvent)
    {
    }
}
