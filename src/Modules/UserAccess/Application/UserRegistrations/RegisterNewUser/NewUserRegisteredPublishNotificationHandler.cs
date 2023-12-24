using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

public class NewUserRegisteredPublishNotificationHandler(IEventBus eventBus)
    : INotificationHandler<NewUserRegisteredNotification>
{
    public async Task Handle(NewUserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        await eventBus.Publish(new NewUserRegisteredIntegrationEvent(
            notification.Id,
            notification.CorrelationId,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.Email,
            notification.DomainEvent.Name,
            notification.DomainEvent.ConfirmationCode.Value,
            notification.DomainEvent.PreferredLanguage.Value));
    }
}
