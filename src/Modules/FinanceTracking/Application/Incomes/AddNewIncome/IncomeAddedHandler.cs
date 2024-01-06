using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;

public class IncomeAddedHandler(UserTagsUpdateService userTags) : INotificationHandler<IncomeAddedDomainEvent>
{
    public Task Handle(IncomeAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        return userTags.UpdateAsync(@event.UserId, @event.Tags);
    }
}
