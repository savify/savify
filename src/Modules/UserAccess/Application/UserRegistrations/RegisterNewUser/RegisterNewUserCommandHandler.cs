using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

internal class RegisterNewUserCommandHandler : ICommandHandler<RegisterNewUserCommand, Guid>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;

    private readonly IUsersCounter _usersCounter;

    public RegisterNewUserCommandHandler(IUserRegistrationRepository userRegistrationRepository, IUsersCounter usersCounter)
    {
        _userRegistrationRepository = userRegistrationRepository;
        _usersCounter = usersCounter;
    }

    public async Task<Guid> Handle(RegisterNewUserCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = UserRegistration.RegisterNewUser(
            command.Email,
            PasswordHasher.HashPassword(command.Password),
            command.Name,
            Country.From(command.Country),
            Language.From(command.PreferredLanguage),
            ConfirmationCode.Generate(),
            _usersCounter);

        await _userRegistrationRepository.AddAsync(userRegistration);

        return userRegistration.Id.Value;
    }
}
