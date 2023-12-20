using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Users.SetNewPassword;

public class SetNewPasswordCommand(Guid userId, string password) : CommandBase
{
    public Guid UserId { get; } = userId;

    public string Password { get; } = password;
}
