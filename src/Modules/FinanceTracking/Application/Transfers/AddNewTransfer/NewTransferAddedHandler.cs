using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
internal class NewTransferAddedHandler(UserTagsUpdateService userTags) : INotificationHandler<TransferAddedDomainEvent>
{
    public Task Handle(TransferAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        return userTags.UpdateAsync(notification.UserId, notification.Tags);
    }
}
