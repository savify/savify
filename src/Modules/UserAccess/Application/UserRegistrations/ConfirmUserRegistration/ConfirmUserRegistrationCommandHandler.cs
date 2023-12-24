using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

internal class ConfirmUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
    : ICommandHandler<ConfirmUserRegistrationCommand>
{
    public async Task Handle(ConfirmUserRegistrationCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = await userRegistrationRepository.GetByIdAsync(
            new UserRegistrationId(command.UserRegistrationId));

        userRegistration.Confirm(ConfirmationCode.From(command.ConfirmationCode));
    }
}
