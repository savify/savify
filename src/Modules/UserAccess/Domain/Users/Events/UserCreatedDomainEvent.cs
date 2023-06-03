using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.Users.Events;

public class UserCreatedDomainEvent : DomainEventBase
{
    public UserId UserId { get; }

    public string Email { get; }

    public string Name { get; }

    public UserRole UserRole { get; }
    
    public Language PreferredLanguage { get; }

    public UserCreatedDomainEvent(
        UserId userId,
        string email,
        string name,
        UserRole userRole,
        Language preferredLanguage)
    {
        UserId = userId;
        Email = email;
        Name = name;
        UserRole = userRole;
        PreferredLanguage = preferredLanguage;
    }
}
