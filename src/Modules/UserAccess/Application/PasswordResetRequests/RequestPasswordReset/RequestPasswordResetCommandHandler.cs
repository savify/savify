using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

internal class RequestPasswordResetCommandHandler(
    IPasswordResetRequestRepository passwordResetRequestRepository,
    IUsersCounter usersCounter)
    : ICommandHandler<RequestPasswordResetCommand, Guid>
{
    public async Task<Guid> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var passwordResetRequest = PasswordResetRequest.Create(
            command.Email,
            ConfirmationCode.Generate(),
            usersCounter);

        await passwordResetRequestRepository.AddAsync(passwordResetRequest);

        return passwordResetRequest.Id.Value;
    }
}
