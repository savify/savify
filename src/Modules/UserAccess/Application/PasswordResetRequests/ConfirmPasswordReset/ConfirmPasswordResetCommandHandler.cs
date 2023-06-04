using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;

internal class ConfirmPasswordResetCommandHandler : ICommandHandler<ConfirmPasswordResetCommand, string>
{
    private readonly IPasswordResetRequestRepository _passwordResetRequestRepository;

    private readonly IUserDetailsProvider _userDetailsProvider;
    
    private readonly IAuthenticationTokenGenerator _authenticationTokenGenerator;

    public ConfirmPasswordResetCommandHandler(
        IPasswordResetRequestRepository passwordResetRequestRepository,
        IUserDetailsProvider userDetailsProvider,
        IAuthenticationTokenGenerator authenticationTokenGenerator)
    {
        _passwordResetRequestRepository = passwordResetRequestRepository;
        _userDetailsProvider = userDetailsProvider;
        _authenticationTokenGenerator = authenticationTokenGenerator;
    }

    public async Task<string> Handle(ConfirmPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var passwordResetRequest = await _passwordResetRequestRepository.GetByIdAsync(
            new PasswordResetRequestId(command.PasswordResetRequestId));
        
        passwordResetRequest.Confirm(ConfirmationCode.From(command.ConfirmationCode));

        var token = _authenticationTokenGenerator.GenerateAccessToken(
            passwordResetRequest.GetUserId(_userDetailsProvider).Value);

        return token.Value;
    }
}
