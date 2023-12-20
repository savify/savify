using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

public class UserRegistrationRenewedPublishNotificationHandler(IEventBus eventBus)
    : INotificationHandler<UserRegistrationRenewedNotification>
{
    public async Task Handle(UserRegistrationRenewedNotification notification, CancellationToken cancellationToken)
    {
        await eventBus.Publish(new UserRegistrationRenewedIntegrationEvent(
            notification.Id,
            notification.CorrelationId,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.Email,
            notification.DomainEvent.Name,
            notification.DomainEvent.ConfirmationCode.Value,
            notification.DomainEvent.PreferredLanguage.Value));
    }
}
