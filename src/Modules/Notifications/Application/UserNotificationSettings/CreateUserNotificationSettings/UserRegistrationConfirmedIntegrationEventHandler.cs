using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

public class UserRegistrationConfirmedIntegrationEventHandler : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    private readonly ICommandScheduler _commandScheduler;

    public UserRegistrationConfirmedIntegrationEventHandler(ICommandScheduler commandScheduler)
    {
        _commandScheduler = commandScheduler;
    }

    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _commandScheduler.EnqueueAsync(new CreateNotificationSettingsCommand(
            @event.Id,
            @event.CorrelationId,
            @event.UserId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));

        await _commandScheduler.EnqueueAsync(new SendUserRegistrationConfirmedEmailCommand(
            Guid.NewGuid(),
            @event.CorrelationId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));
    }
}
