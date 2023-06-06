using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

public class UserRegistrationConfirmedIntegrationEventHandler : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    private readonly ICommandScheduler _commandScheduler;

    public UserRegistrationConfirmedIntegrationEventHandler(ICommandScheduler commandScheduler)
    {
        _commandScheduler = commandScheduler;
    }

    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _commandScheduler.EnqueueAsync(new SendUserRegistrationConfirmedEmailCommand(
            @event.Id,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));
    }
}
