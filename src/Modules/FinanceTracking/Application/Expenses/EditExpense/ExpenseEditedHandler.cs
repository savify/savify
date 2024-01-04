using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Expenses.EditExpense;

public class ExpenseEditedHandler(UserTagsUpdateService userTags) : INotificationHandler<ExpenseEditedDomainEvent>
{
    public Task Handle(ExpenseEditedDomainEvent @event, CancellationToken cancellationToken)
    {
        return userTags.UpdateAsync(@event.UserId, @event.Tags);
    }
}
