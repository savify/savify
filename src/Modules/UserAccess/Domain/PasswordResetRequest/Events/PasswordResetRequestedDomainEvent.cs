using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Events;

public class PasswordResetRequestedDomainEvent : DomainEventBase
{
    public string UserEmail { get; }
    
    public ConfirmationCode ConfirmationCode { get; }
    
    public DateTime ExpiresAt { get; }

    public PasswordResetRequestedDomainEvent(string userEmail, ConfirmationCode confirmationCode, DateTime expiresAt)
    {
        UserEmail = userEmail;
        ConfirmationCode = confirmationCode;
        ExpiresAt = expiresAt;
    }
}
