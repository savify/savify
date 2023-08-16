using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class PasswordResetRequestedIntegrationEvent : IntegrationEvent
{
    public string Email { get; }

    public string ConfirmationCode { get; }

    public PasswordResetRequestedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        string email,
        string confirmationCode) : base(id, occurredOn)
    {
        Email = email;
        ConfirmationCode = confirmationCode;
    }
}
