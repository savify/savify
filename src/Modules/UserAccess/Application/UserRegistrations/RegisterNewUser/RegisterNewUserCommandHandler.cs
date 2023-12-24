using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

internal class RegisterNewUserCommandHandler(
    IUserRegistrationRepository userRegistrationRepository,
    IUsersCounter usersCounter)
    : ICommandHandler<RegisterNewUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterNewUserCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = UserRegistration.RegisterNewUser(
            command.Email,
            PasswordHasher.HashPassword(command.Password),
            command.Name,
            Country.From(command.Country),
            Language.From(command.PreferredLanguage),
            ConfirmationCode.Generate(),
            usersCounter);

        await userRegistrationRepository.AddAsync(userRegistration);

        return userRegistration.Id.Value;
    }
}
