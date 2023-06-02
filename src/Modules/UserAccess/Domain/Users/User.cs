using App.BuildingBlocks.Domain;
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

    private bool _isActive;

    private DateTime _createdAt;

    public static User Create(
        string email,
        string password,
        string name,
        UserRole role,
        IUsersCounter usersCounter)
    {
        CheckRules(new UserEmailMustBeUniqueRule(usersCounter, email));
        
        return new User(email, password, name, role);
    }

    private User(string email, string password, string name, UserRole role)
    {
        Id = new UserId(Guid.NewGuid());
        _email = email;
        _password = password;
        _name = name;
        _isActive = true;
        _createdAt = DateTime.UtcNow;

        _roles = new List<UserRole>();
        _roles.Add(role);

        AddDomainEvent(new UserCreatedDomainEvent(Id, email, name, role));
    }
    
    private User() {}
}
