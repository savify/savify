using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Events;

public class PasswordResetRequestedDomainEvent : DomainEventBase
{
    public string UserEmail { get; }

    public ConfirmationCode ConfirmationCode { get; }

    public DateTime ValidTill { get; }

    public PasswordResetRequestedDomainEvent(string userEmail, ConfirmationCode confirmationCode, DateTime validTill)
    {
        UserEmail = userEmail;
        ConfirmationCode = confirmationCode;
        ValidTill = validTill;
    }
}
