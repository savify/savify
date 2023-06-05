using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

internal class RequestPasswordResetCommandHandler : ICommandHandler<RequestPasswordResetCommand, Guid>
{
    private readonly IPasswordResetRequestRepository _passwordResetRequestRepository;

    private readonly IUsersCounter _usersCounter;

    public RequestPasswordResetCommandHandler(IPasswordResetRequestRepository passwordResetRequestRepository, IUsersCounter usersCounter)
    {
        _passwordResetRequestRepository = passwordResetRequestRepository;
        _usersCounter = usersCounter;
    }

    public async Task<Guid> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var passwordResetRequest = PasswordResetRequest.Create(
            command.Email,
            ConfirmationCode.Generate(),
            _usersCounter);

        await _passwordResetRequestRepository.AddAsync(passwordResetRequest);

        return passwordResetRequest.Id.Value;
    }
}
