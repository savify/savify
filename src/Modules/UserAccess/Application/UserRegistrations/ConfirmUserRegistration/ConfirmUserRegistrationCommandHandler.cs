using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.UserRegistrations.Specifications;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class ConfirmUserRegistrationCommandHandler : ICommandHandler<ConfirmUserRegistrationCommand, Result>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;

    public ConfirmUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
    {
        _userRegistrationRepository = userRegistrationRepository;
    }

    public async Task<Result> Handle(ConfirmUserRegistrationCommand command, CancellationToken cancellationToken)
    {
        var userRegistration = await _userRegistrationRepository.GetByIdAsync(
            new UserRegistrationId(command.UserRegistrationId));
        
        userRegistration.Confirm(ConfirmationCode.From(command.ConfirmationCode), new ConfirmationDateSpecification());
        
        return Result.Success;
    }
}
