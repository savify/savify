using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.EditIncome;

public class IncomeEditedUpdateUserTagsHandler(UserTagsUpdateService userTags) : INotificationHandler<IncomeEditedDomainEvent>
{
    public Task Handle(IncomeEditedDomainEvent @event, CancellationToken cancellationToken)
    {
        return userTags.UpdateAsync(@event.UserId, @event.Tags);
    }
}
