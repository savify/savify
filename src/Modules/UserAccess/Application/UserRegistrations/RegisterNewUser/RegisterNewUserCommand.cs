using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

public class RegisterNewUserCommand(
    string email,
    string password,
    string name,
    string country,
    string preferredLanguage)
    : CommandBase<Guid>
{
    [LogMasked]
    public string Email { get; } = email;

    [LogMasked]
    public string Password { get; } = password;

    [LogMasked]
    public string Name { get; } = name;

    public string Country { get; } = country;

    public string PreferredLanguage { get; } = preferredLanguage;
}
