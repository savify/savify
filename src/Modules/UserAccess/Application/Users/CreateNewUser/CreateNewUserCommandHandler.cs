using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

internal class CreateNewUserCommandHandler : ICommandHandler<CreateNewUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    private readonly IUsersCounter _usersCounter;
    
    public CreateNewUserCommandHandler(IUserRepository userRepository, IUsersCounter usersCounter)
    {
        _userRepository = userRepository;
        _usersCounter = usersCounter;
    }

    public async Task<Guid> Handle(CreateNewUserCommand command, CancellationToken cancellationToken)
    {
        var password = PasswordHasher.HashPassword(command.Password);

        var user = User.Create(
            command.Email,
            password,
            command.Name,
            UserRole.From(command.Role),
            Country.From(command.Country),
            _usersCounter);

        await _userRepository.AddAsync(user);

        return user.Id.Value;
    }
}
