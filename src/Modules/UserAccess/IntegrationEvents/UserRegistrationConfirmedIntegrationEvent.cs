using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class UserRegistrationConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; }
    
    public string Email { get; }
    
    public string Name { get; }

    public string PreferredLanguage { get; }
    
    public UserRegistrationConfirmedIntegrationEvent(Guid id,
        DateTime occurredOn,
        Guid userId,
        string email,
        string name,
        string preferredLanguage) : base(id, occurredOn)
    {
        UserId = userId;
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
    }
}
