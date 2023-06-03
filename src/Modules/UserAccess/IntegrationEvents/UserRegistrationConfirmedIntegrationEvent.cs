using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class UserRegistrationConfirmedIntegrationEvent : IntegrationEvent
{
    public string Email { get; }
    
    public string Name { get; }

    public string PreferredLanguage { get; }
    
    public UserRegistrationConfirmedIntegrationEvent(Guid id,
        DateTime occurredOn,
        string email,
        string name,
        string preferredLanguage) : base(id, occurredOn)
    {
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
    }
}
