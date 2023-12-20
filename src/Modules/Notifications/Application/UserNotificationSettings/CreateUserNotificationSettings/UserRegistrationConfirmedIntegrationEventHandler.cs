using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

public class UserRegistrationConfirmedIntegrationEventHandler(ICommandScheduler commandScheduler)
    : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await commandScheduler.EnqueueAsync(new CreateNotificationSettingsCommand(
            @event.Id,
            @event.CorrelationId,
            @event.UserId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));

        await commandScheduler.EnqueueAsync(new SendUserRegistrationConfirmedEmailCommand(
            Guid.NewGuid(),
            @event.CorrelationId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));
    }
}
