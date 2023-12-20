using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class PasswordResetRequestedIntegrationEvent(
    Guid id,
    Guid correlationId,
    DateTime occurredOn,
    string email,
    string confirmationCode)
    : IntegrationEvent(id, correlationId, occurredOn)
{
    public string Email { get; } = email;

    public string ConfirmationCode { get; } = confirmationCode;
}
