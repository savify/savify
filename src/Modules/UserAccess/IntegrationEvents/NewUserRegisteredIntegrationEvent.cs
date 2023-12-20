using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class NewUserRegisteredIntegrationEvent(
    Guid id,
    Guid correlationId,
    DateTime occurredOn,
    string email,
    string name,
    string confirmationCode,
    string preferredLanguage)
    : IntegrationEvent(id, correlationId, occurredOn)
{
    public string Email { get; } = email;

    public string Name { get; } = name;

    public string ConfirmationCode { get; } = confirmationCode;

    public string PreferredLanguage { get; } = preferredLanguage;
}
