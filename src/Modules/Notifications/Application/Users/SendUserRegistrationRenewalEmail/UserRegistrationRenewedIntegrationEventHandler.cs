using App.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

public class UserRegistrationRenewedIntegrationEventHandler : INotificationHandler<UserRegistrationRenewedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public UserRegistrationRenewedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UserRegistrationRenewedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendUserRegistrationRenewalEmailCommand(
            @event.Id,
            @event.Name,
            @event.Email,
            @event.PreferredLanguage,
            @event.ConfirmationCode));
    }
}
