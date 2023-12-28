using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;
internal class TransferEditedHandler(UserTagsUpdateService service) : INotificationHandler<TransferEditedDomainEvent>
{
    public Task Handle(TransferEditedDomainEvent notification, CancellationToken cancellationToken)
    {
        return service.UpdateAsync(notification.UserId, notification.Tags);
    }
}
