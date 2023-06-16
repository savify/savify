using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

public class CreateNewUserCommand : CommandBase<Guid>
{
    [LogMasked]
    public string Email { get; }
    
    [LogMasked]
    public string Password { get; }
    
    [LogMasked]
    public string Name { get; }
    
    public string Role { get; }
    
    public string Country { get; }

    public CreateNewUserCommand(string email, string password, string name, string role, string country)
    {
        Email = email;
        Password = password;
        Name = name;
        Role = role;
        Country = country;
    }
}
