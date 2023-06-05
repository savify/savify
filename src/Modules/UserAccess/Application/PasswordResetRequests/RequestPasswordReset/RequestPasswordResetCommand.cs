using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

public class RequestPasswordResetCommand : CommandBase<Guid>
{
    [LogMasked]
    public string Email { get; }

    public RequestPasswordResetCommand(string email)
    {
        Email = email;
    }
}
