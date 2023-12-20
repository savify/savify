using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

public class CreateNewUserCommand(string email, string password, string name, string role, string country)
    : CommandBase<Guid>
{
    [LogMasked]
    public string Email { get; } = email;

    [LogMasked]
    public string Password { get; } = password;

    [LogMasked]
    public string Name { get; } = name;

    public string Role { get; } = role;

    public string Country { get; } = country;
}
