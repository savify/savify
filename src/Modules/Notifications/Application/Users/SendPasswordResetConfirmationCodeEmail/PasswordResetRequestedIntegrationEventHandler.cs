using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class PasswordResetRequestedIntegrationEventHandler : INotificationHandler<PasswordResetRequestedIntegrationEvent>
{
    private readonly ICommandScheduler _commandScheduler;

    public PasswordResetRequestedIntegrationEventHandler(ICommandScheduler commandScheduler)
    {
        _commandScheduler = commandScheduler;
    }

    public async Task Handle(PasswordResetRequestedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _commandScheduler.EnqueueAsync(new SendPasswordResetConfirmationCodeEmailCommand(
            @event.Id,
            @event.Email,
            @event.ConfirmationCode));
    }
}
