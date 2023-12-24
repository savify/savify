using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

public class PasswordResetRequestedPublishNotificationHandler(IEventBus eventBus) : INotificationHandler<PasswordResetRequestedNotification>
{
    public async Task Handle(PasswordResetRequestedNotification notification, CancellationToken cancellationToken)
    {
        await eventBus.Publish(new PasswordResetRequestedIntegrationEvent(
            notification.Id,
            notification.CorrelationId,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.UserEmail,
            notification.DomainEvent.ConfirmationCode.Value));
    }
}
