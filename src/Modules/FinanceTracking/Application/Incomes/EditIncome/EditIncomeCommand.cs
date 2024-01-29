using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Incomes.EditIncome;

public class EditIncomeCommand(Guid incomeId, Guid userId, Guid targetWalletId, Guid categoryId, int amount, DateTime madeOn, string? comment, IEnumerable<string>? tags)
    : CommandBase
{
    public Guid IncomeId { get; } = incomeId;

    public Guid UserId { get; } = userId;

    public Guid TargetWalletId { get; } = targetWalletId;

    public Guid CategoryId { get; } = categoryId;

    public int Amount { get; } = amount;

    public DateTime MadeOn { get; } = madeOn;

    public string? Comment { get; } = comment;

    public IEnumerable<string>? Tags { get; } = tags;
}
