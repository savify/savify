using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Events;

public class PasswordResetRequestConfirmedDomainEvent : DomainEventBase
{
    public PasswordResetRequestId PasswordResetRequestId { get; }

    public PasswordResetRequestConfirmedDomainEvent(PasswordResetRequestId passwordResetRequestId)
    {
        PasswordResetRequestId = passwordResetRequestId;
    }
}
