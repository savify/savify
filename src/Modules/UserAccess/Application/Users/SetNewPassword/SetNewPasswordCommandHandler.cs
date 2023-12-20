using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.Users.SetNewPassword;

internal class SetNewPasswordCommandHandler(IUserRepository userRepository) : ICommandHandler<SetNewPasswordCommand>
{
    public async Task Handle(SetNewPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(command.UserId));
        var password = PasswordHasher.HashPassword(command.Password);

        user.SetNewPassword(password);
    }
}
