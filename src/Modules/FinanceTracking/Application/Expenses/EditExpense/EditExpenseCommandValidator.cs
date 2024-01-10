using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using App.Modules.FinanceTracking.Domain.Categories;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Expenses.EditExpense;

internal class EditExpenseCommandValidator : Validator<EditExpenseCommand>
{
    public EditExpenseCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(c => c.ExpenseId)
            .NotEmpty()
            .WithMessage("Please provide expense id");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("Please provide user id");

        RuleFor(c => c.SourceWalletId)
            .NotEmpty()
            .WithMessage("Please provide source wallet id");

        RuleFor(c => c.CategoryId)
            .NotEmpty()
            .WithMessage("Please provide expense category id")
            .MustAsync(async (categoryId, _) =>
                await categoryRepository.ExistsWithIdAsync(new CategoryId(categoryId)))
            .WithMessage("Category with provided id does not exist");

        RuleFor(c => c.Amount)
            .GreaterThan(0)
            .WithMessage("Expense amount must be bigger than 0");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide expense currency")
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.MadeOn)
            .NotEmpty()
            .WithMessage("Please provide expense's made on date");
    }
}
