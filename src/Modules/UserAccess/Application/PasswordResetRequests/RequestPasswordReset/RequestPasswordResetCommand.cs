using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

public class RequestPasswordResetCommand(string email) : CommandBase<Guid>
{
    [LogMasked]
    public string Email { get; } = email;
}
