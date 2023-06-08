using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class PasswordResetRequestedIntegrationEventHandler : INotificationHandler<PasswordResetRequestedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public PasswordResetRequestedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(PasswordResetRequestedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendPasswordResetConfirmationCodeEmailCommand(
            @event.Id,
            @event.Email,
            @event.ConfirmationCode));
    }
}
