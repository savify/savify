using App.BuildingBlocks.Integration;
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
        // TODO: add subscriber to integration event
        // await _eventBus.Publish(new UserRegistrationConfirmedIntegrationEvent(
        //     notification.Id,
        //     notification.DomainEvent.OccurredOn,
        //     notification.DomainEvent.Email,
        //     notification.DomainEvent.Name,
        //     notification.DomainEvent.PreferredLanguage.Value));
    }
}