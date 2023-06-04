using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Users.SetNewPassword;

public class SetNewPasswordCommand : CommandBase<Result>
{
    public Guid UserId { get; }
    
    public string Password { get; }

    public SetNewPasswordCommand(Guid userId, string password)
    {
        UserId = userId;
        Password = password;
    }
}
