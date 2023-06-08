using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

public class NewUserRegisteredIntegrationEventHandler : INotificationHandler<NewUserRegisteredIntegrationEvent>
{
    private readonly IMediator _mediator;

    public NewUserRegisteredIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(NewUserRegisteredIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendUserRegistrationConfirmationEmailCommand(
            @event.Id,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage,
            @event.ConfirmationCode));
    }
}
