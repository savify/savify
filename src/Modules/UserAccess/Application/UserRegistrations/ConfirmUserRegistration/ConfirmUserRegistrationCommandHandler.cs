using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

internal class ConfirmUserRegistrationCommandHandler : ICommandHandler<ConfirmUserRegistrationCommand>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;

    public ConfirmUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
    {
        _userRegistrationRepository = userRegistrationRepository;
    }

    public async Task Handle(ConfirmUserRegistrationCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = await _userRegistrationRepository.GetByIdAsync(
            new UserRegistrationId(command.UserRegistrationId));

        userRegistration.Confirm(ConfirmationCode.From(command.ConfirmationCode));
    }
}
