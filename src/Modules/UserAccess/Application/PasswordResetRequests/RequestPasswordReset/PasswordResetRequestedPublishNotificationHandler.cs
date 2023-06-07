using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

public class PasswordResetRequestedPublishNotificationHandler : INotificationHandler<PasswordResetRequestedNotification>
{
    private readonly IEventBus _eventBus;

    public PasswordResetRequestedPublishNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PasswordResetRequestedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new PasswordResetRequestedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.UserEmail,
            notification.DomainEvent.ConfirmationCode.Value));
    }
}
