using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

public class UserRegistrationRenewedIntegrationEventHandler(ICommandScheduler commandScheduler) : INotificationHandler<UserRegistrationRenewedIntegrationEvent>
{
    public async Task Handle(UserRegistrationRenewedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await commandScheduler.EnqueueAsync(new SendUserRegistrationRenewalEmailCommand(
            @event.Id,
            @event.CorrelationId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage,
            @event.ConfirmationCode));
    }
}
