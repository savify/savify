using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

internal class RenewUserRegistrationCommandHandler : ICommandHandler<RenewUserRegistrationCommand>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;

    public RenewUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
    {
        _userRegistrationRepository = userRegistrationRepository;
    }

    public async Task Handle(RenewUserRegistrationCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = await _userRegistrationRepository.GetByIdAsync(
            new UserRegistrationId(command.UserRegistrationId));

        userRegistration.Renew(ConfirmationCode.Generate());
    }
}
