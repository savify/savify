using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class UserRegistrationConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; }

    public string Email { get; }

    public string Name { get; }

    public string PreferredLanguage { get; }

    public UserRegistrationConfirmedIntegrationEvent(
        Guid id,
        Guid correlationId,
        DateTime occurredOn,
        Guid userId,
        string email,
        string name,
        string preferredLanguage) : base(id, correlationId, occurredOn)
    {
        UserId = userId;
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
    }
}
