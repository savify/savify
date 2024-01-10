using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;

internal class RemoveIncomeCommandValidator : Validator<RemoveIncomeCommand>
{
    public RemoveIncomeCommandValidator()
    {
        RuleFor(c => c.IncomeId)
            .NotEmpty()
            .WithMessage("Please provide income id");
    }
}
