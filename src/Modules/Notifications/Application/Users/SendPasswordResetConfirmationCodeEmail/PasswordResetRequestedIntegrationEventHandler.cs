using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class PasswordResetRequestedIntegrationEventHandler(ICommandScheduler commandScheduler)
    : INotificationHandler<PasswordResetRequestedIntegrationEvent>
{
    public async Task Handle(PasswordResetRequestedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await commandScheduler.EnqueueAsync(new SendPasswordResetConfirmationCodeEmailCommand(
            @event.Id,
            @event.CorrelationId,
            @event.Email,
            @event.ConfirmationCode));
    }
}
