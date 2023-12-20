using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using App.Modules.UserAccess.Domain.Users;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class UserRegistrationConfirmedHandler(
    IUserRegistrationRepository userRegistrationRepository,
    IUserRepository userRepository)
    : INotificationHandler<UserRegistrationConfirmedDomainEvent>
{
    public async Task Handle(UserRegistrationConfirmedDomainEvent @event, CancellationToken cancellationToken)
    {
        var userRegistration = await userRegistrationRepository.GetByIdAsync(@event.UserRegistrationId);

        var user = userRegistration.CreateUser();

        await userRepository.AddAsync(user);
    }
}
