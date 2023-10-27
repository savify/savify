using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.Users.SetNewPassword;

internal class SetNewPasswordCommandHandler : ICommandHandler<SetNewPasswordCommand>
{
    private readonly IUserRepository _userRepository;

    public SetNewPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(SetNewPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(command.UserId));
        var password = PasswordHasher.HashPassword(command.Password);

        user.SetNewPassword(password);
    }
}
