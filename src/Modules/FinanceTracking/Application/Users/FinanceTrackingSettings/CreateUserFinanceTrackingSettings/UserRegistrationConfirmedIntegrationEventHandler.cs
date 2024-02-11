using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.CreateUserFinanceTrackingSettings;

public class UserRegistrationConfirmedIntegrationEventHandler(ICommandScheduler commandScheduler)
    : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await commandScheduler.EnqueueAsync(new CreateUserFinanceTrackingSettingsCommand(
            @event.Id,
            @event.CorrelationId,
            @event.UserId,
            @event.Country,
            @event.PreferredLanguage));
    }
}
