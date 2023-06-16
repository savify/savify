using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.Users.Events;
using App.Modules.UserAccess.Domain.Users.Rules;

namespace App.Modules.UserAccess.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public UserId Id { get; private set; }
    
    private string _email;

    private string _password;

    private string _name;

    private List<UserRole> _roles;

    private Country _country;
    
    private Language _preferredLanguage;

    private bool _isActive;

    private DateTime _createdAt;

    public static User Create(
        string email,
        string password,
        string name,
        UserRole role,
        Country country,
        IUsersCounter usersCounter)
    {
        CheckRules(new UserEmailMustBeUniqueRule(usersCounter, email));
        
        return new User(new UserId(Guid.NewGuid()), email, password, name, role, country, Language.From("en"));
    }

    internal static User CreateFromUserRegistration(UserRegistrationId id, string email, string password, string name, Country country, Language preferredLanguage)
    {
        return new User(new UserId(id.Value), email, password, name, UserRole.User, country, preferredLanguage);
    }

    public void SetNewPassword(string password)
    {
        _password = password;
        
        AddDomainEvent(new NewPasswordWasSetDomainEvent(Id, _email, _name, _preferredLanguage));
    }

    private User(UserId id, string email, string password, string name, UserRole role, Country country, Language preferredLanguage)
    {
        Id = id;
        _email = email;
        _password = password;
        _name = name;
        _country = country;
        _preferredLanguage = preferredLanguage;
        _isActive = true;
        _createdAt = DateTime.UtcNow;

        _roles = new List<UserRole>();
        _roles.Add(role);

        AddDomainEvent(new UserCreatedDomainEvent(Id, _email, _name, role, country, _preferredLanguage));
    }
    
    private User() {}
}
