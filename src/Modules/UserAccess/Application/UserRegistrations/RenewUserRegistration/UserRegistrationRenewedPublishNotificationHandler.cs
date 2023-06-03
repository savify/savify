using MediatR;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

public class UserRegistrationRenewedPublishNotificationHandler : INotificationHandler<UserRegistrationRenewedNotification>
{
    public async Task Handle(UserRegistrationRenewedNotification notification, CancellationToken cancellationToken)
    {
        // TODO: add subscriber to integration event
        // await _eventBus.Publish(new UserRegistrationRenewedIntegrationEvent(
        //     notification.Id,
        //     notification.DomainEvent.OccurredOn,
        //     notification.DomainEvent.Email,
        //     notification.DomainEvent.Name,
        //     notification.DomainEvent.ConfirmationCode.Value,
        //     notification.DomainEvent.PreferredLanguage.Value));
    }
}
