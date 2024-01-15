using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;

public class AddNewExpenseCommand(Guid userId, Guid sourceWalletId, Guid categoryId, int amount, DateTime madeOn, string? comment, IEnumerable<string>? tags)
    : CommandBase<Guid>
{
    public Guid UserId { get; } = userId;

    public Guid SourceWalletId { get; } = sourceWalletId;

    public Guid CategoryId { get; } = categoryId;

    public int Amount { get; } = amount;

    public DateTime MadeOn { get; } = madeOn;

    public string? Comment { get; } = comment;

    public IEnumerable<string>? Tags { get; } = tags;
}
