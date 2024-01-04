using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;

public class AddNewExpenseCommandValidator : Validator<AddNewExpenseCommand>
{
    public AddNewExpenseCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("Please provide user id");

        RuleFor(c => c.SourceWalletId)
            .NotEmpty()
            .WithMessage("Please provide source wallet id");

        RuleFor(c => c.CategoryId)
            .NotEmpty()
            .WithMessage("Please provide expense category id");

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
