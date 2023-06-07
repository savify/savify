using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class UserRegistrationConfirmedPublishNotificationHandler : INotificationHandler<UserRegistrationConfirmedNotification>
{
    private readonly IEventBus _eventBus;

    public UserRegistrationConfirmedPublishNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(UserRegistrationConfirmedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new UserRegistrationConfirmedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.UserRegistrationId.Value,
            notification.DomainEvent.Email,
            notification.DomainEvent.Name,
            notification.DomainEvent.PreferredLanguage.Value));
    }
}
