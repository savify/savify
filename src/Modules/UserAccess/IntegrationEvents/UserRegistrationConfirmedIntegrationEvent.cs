using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class UserRegistrationConfirmedIntegrationEvent(
    Guid id,
    Guid correlationId,
    DateTime occurredOn,
    Guid userId,
    string email,
    string name,
    string country,
    string preferredLanguage)
    : IntegrationEvent(id, correlationId, occurredOn)
{
    public Guid UserId { get; } = userId;

    public string Email { get; } = email;

    public string Name { get; } = name;

    public string Country { get; } = country;

    public string PreferredLanguage { get; } = preferredLanguage;
}
