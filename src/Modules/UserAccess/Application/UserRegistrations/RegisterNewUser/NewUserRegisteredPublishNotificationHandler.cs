using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

public class NewUserRegisteredPublishNotificationHandler : INotificationHandler<NewUserRegisteredNotification>
{
    private readonly IEventBus _eventBus;

    public NewUserRegisteredPublishNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(NewUserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        // TODO: add subscriber to integration event
        // await _eventBus.Publish(new NewUserRegisteredIntegrationEvent(
        //     notification.Id,
        //     notification.DomainEvent.OccurredOn,
        //     notification.DomainEvent.Email,
        //     notification.DomainEvent.Name,
        //     notification.DomainEvent.ConfirmationCode.Value,
        //     notification.DomainEvent.PreferredLanguage.Value));
    }
}
