using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

public class NewUserRegisteredIntegrationEventHandler : INotificationHandler<NewUserRegisteredIntegrationEvent>
{
    private readonly ICommandScheduler _commandScheduler;

    public NewUserRegisteredIntegrationEventHandler(ICommandScheduler commandScheduler)
    {
        _commandScheduler = commandScheduler;
    }

    public async Task Handle(NewUserRegisteredIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _commandScheduler.EnqueueAsync(new SendUserRegistrationConfirmationEmailCommand(
            @event.Id,
            @event.CorrelationId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage,
            @event.ConfirmationCode));
    }
}
