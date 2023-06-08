using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

public class UserRegistrationConfirmedIntegrationEventHandler : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public UserRegistrationConfirmedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UserRegistrationConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendUserRegistrationConfirmedEmailCommand(
            Guid.NewGuid(),
            @event.Name,
            @event.Email,
            @event.PreferredLanguage));
    }
}
