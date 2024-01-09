using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Events;

public class PasswordResetFinishedDomainEvent(PasswordResetRequestId passwordResetRequestId) : DomainEventBase
{
    public PasswordResetRequestId PasswordResetRequestId { get; } = passwordResetRequestId;
}
