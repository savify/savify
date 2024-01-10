using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;

public class ExpenseAddedUpdateUserTagsHandler(UserTagsUpdateService userTags) : INotificationHandler<ExpenseAddedDomainEvent>
{
    public Task Handle(ExpenseAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        return userTags.UpdateAsync(@event.UserId, @event.Tags);
    }
}
