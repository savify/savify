using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class NewUserRegisteredIntegrationEvent : IntegrationEvent
{
    public string Email { get; }
    
    public string Name { get; }
    
    public string ConfirmationCode { get; }
    
    public string PreferredLanguage { get; }
    
    public NewUserRegisteredIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        string email,
        string name,
        string confirmationCode,
        string preferredLanguage) : base(id, occurredOn)
    {
        Email = email;
        Name = name;
        ConfirmationCode = confirmationCode;
        PreferredLanguage = preferredLanguage;
    }
}
