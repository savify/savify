using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

internal class RenewUserRegistrationCommandHandler : ICommandHandler<RenewUserRegistrationCommand, Result>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;

    public RenewUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
    {
        _userRegistrationRepository = userRegistrationRepository;
    }
    
    public async Task<Result> Handle(RenewUserRegistrationCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = await _userRegistrationRepository.GetByIdAsync(
            new UserRegistrationId(command.UserRegistrationId));
        
        userRegistration.Renew(ConfirmationCode.Generate());
        
        return Result.Success;
    }
}
