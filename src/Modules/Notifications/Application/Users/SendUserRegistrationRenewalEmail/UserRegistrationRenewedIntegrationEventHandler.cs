using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

public class UserRegistrationRenewedIntegrationEventHandler : INotificationHandler<UserRegistrationRenewedIntegrationEvent>
{
    private readonly ICommandScheduler _commandScheduler;

    public UserRegistrationRenewedIntegrationEventHandler(ICommandScheduler commandScheduler)
    {
        _commandScheduler = commandScheduler;
    }

    public async Task Handle(UserRegistrationRenewedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _commandScheduler.EnqueueAsync(new SendUserRegistrationRenewalEmailCommand(
            @event.Id,
            @event.CorrelationId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage,
            @event.ConfirmationCode));
    }
}
