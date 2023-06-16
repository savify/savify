using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Domain.Users;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

public class RegisterNewUserCommand : CommandBase<Guid>
{
    [LogMasked]
    public string Email { get; }
    
    [LogMasked]
    public string Password { get; }
    
    [LogMasked]
    public string Name { get; }
    
    public string Country { get; }
    
    public string PreferredLanguage { get; }

    public RegisterNewUserCommand(string email, string password, string name, string country, string preferredLanguage)
    {
        Email = email;
        Password = password;
        Name = name;
        Country = country;
        PreferredLanguage = preferredLanguage;
    }
}
