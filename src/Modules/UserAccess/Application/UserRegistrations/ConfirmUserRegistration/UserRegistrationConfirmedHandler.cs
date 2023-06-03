using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using App.Modules.UserAccess.Domain.Users;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class UserRegistrationConfirmedHandler : INotificationHandler<UserRegistrationConfirmedDomainEvent>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;

    private readonly IUserRepository _userRepository;

    public UserRegistrationConfirmedHandler(IUserRegistrationRepository userRegistrationRepository, IUserRepository userRepository)
    {
        _userRegistrationRepository = userRegistrationRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(UserRegistrationConfirmedDomainEvent @event, CancellationToken cancellationToken)
    {
        var userRegistration = await _userRegistrationRepository.GetByIdAsync(@event.UserRegistrationId);

        var user = userRegistration.CreateUser();

        await _userRepository.AddAsync(user);
    }
}
