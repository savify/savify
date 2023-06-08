using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

public class UserRegistrationsConfirmedIntegrationEventHandler : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public UserRegistrationsConfirmedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateNotificationSettingsCommand(
            @event.Id,
            @event.UserId,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));
    }
}
