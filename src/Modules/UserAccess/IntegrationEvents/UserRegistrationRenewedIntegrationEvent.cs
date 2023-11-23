using App.BuildingBlocks.Integration;

namespace App.Modules.UserAccess.IntegrationEvents;

public class UserRegistrationRenewedIntegrationEvent : IntegrationEvent
{
    public string Email { get; }

    public string Name { get; }

    public string ConfirmationCode { get; }

    public string PreferredLanguage { get; }

    public UserRegistrationRenewedIntegrationEvent(
        Guid id,
        Guid correlationId,
        DateTime occurredOn,
        string email,
        string name,
        string confirmationCode,
        string preferredLanguage) : base(id, correlationId, occurredOn)
    {
        Email = email;
        Name = name;
        ConfirmationCode = confirmationCode;
        PreferredLanguage = preferredLanguage;
    }
}
