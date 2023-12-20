using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

internal class CreateNewUserCommandHandler(
    IUserRepository userRepository,
    IUsersCounter usersCounter)
    : ICommandHandler<CreateNewUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateNewUserCommand command, CancellationToken cancellationToken)
    {
        var password = PasswordHasher.HashPassword(command.Password);

        var user = User.Create(
            command.Email,
            password,
            command.Name,
            UserRole.From(command.Role),
            Country.From(command.Country),
            usersCounter);

        await userRepository.AddAsync(user);

        return user.Id.Value;
    }
}
