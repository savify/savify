using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;

public class RemoveExpenseCommandValidator : Validator<RemoveExpenseCommand>
{
    public RemoveExpenseCommandValidator()
    {
        RuleFor(c => c.ExpenseId)
            .NotEmpty()
            .WithMessage("Please provide expense id");
    }
}
