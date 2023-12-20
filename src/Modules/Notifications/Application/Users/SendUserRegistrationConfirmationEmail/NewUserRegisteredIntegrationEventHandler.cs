using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

public class NewUserRegisteredIntegrationEventHandler(ICommandScheduler commandScheduler)
    : INotificationHandler<NewUserRegisteredIntegrationEvent>
{
    public async Task Handle(NewUserRegisteredIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await commandScheduler.EnqueueAsync(new SendUserRegistrationConfirmationEmailCommand(
            @event.Id,
            @event.CorrelationId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage,
            @event.ConfirmationCode));
    }
}
