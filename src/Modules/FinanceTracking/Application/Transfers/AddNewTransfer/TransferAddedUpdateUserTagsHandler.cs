using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class TransferAddedUpdateUserTagsHandler(UserTagsUpdateService userTags) : INotificationHandler<TransferAddedDomainEvent>
{
    public Task Handle(TransferAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        return userTags.UpdateAsync(@event.UserId, @event.Tags);
    }
}
