using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Expenses.EditExpense;

public class EditExpenseCommand(Guid expenseId, Guid userId, Guid sourceWalletId, Guid categoryId, int amount, string currency, DateTime madeOn, string? comment, IEnumerable<string>? tags)
    : CommandBase
{
    public Guid ExpenseId { get; } = expenseId;

    public Guid UserId { get; } = userId;

    public Guid SourceWalletId { get; } = sourceWalletId;

    public Guid CategoryId { get; } = categoryId;

    public int Amount { get; } = amount;

    public string Currency { get; } = currency;

    public DateTime MadeOn { get; } = madeOn;

    public string? Comment { get; } = comment;

    public IEnumerable<string>? Tags { get; } = tags;
}
