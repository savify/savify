using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

public class UserRegistrationsConfirmedIntegrationEventHandler : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    private readonly ICommandScheduler _commandScheduler;

    public UserRegistrationsConfirmedIntegrationEventHandler(ICommandScheduler commandScheduler)
    {
        _commandScheduler = commandScheduler;
    }

    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _commandScheduler.EnqueueAsync(new CreateNotificationSettingsCommand(
            @event.Id,
            @event.UserId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));
    }
}
