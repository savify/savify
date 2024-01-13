using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class UserRegistrationConfirmedPublishNotificationHandler(IEventBus eventBus)
    : INotificationHandler<UserRegistrationConfirmedNotification>
{
    public async Task Handle(UserRegistrationConfirmedNotification notification, CancellationToken cancellationToken)
    {
        await eventBus.Publish(new UserRegistrationConfirmedIntegrationEvent(
            notification.Id,
            notification.CorrelationId,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.UserRegistrationId.Value,
            notification.DomainEvent.Email,
            notification.DomainEvent.Name,
            notification.DomainEvent.Country.Value,
            notification.DomainEvent.PreferredLanguage.Value));
    }
}
