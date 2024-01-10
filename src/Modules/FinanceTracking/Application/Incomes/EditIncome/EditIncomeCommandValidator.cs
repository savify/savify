using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using App.Modules.FinanceTracking.Domain.Categories;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Incomes.EditIncome;

internal class EditIncomeCommandValidator : Validator<EditIncomeCommand>
{
    public EditIncomeCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(c => c.IncomeId)
            .NotEmpty()
            .WithMessage("Please provide income id");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("Please provide user id");

        RuleFor(c => c.TargetWalletId)
            .NotEmpty()
            .WithMessage("Please provide target wallet id");

        RuleFor(c => c.CategoryId)
            .NotEmpty()
            .WithMessage("Please provide income category id")
            .MustAsync(async (categoryId, _) =>
                await categoryRepository.ExistsWithIdAsync(new CategoryId(categoryId)))
            .WithMessage("Category with provided id does not exist");

        RuleFor(c => c.Amount)
            .GreaterThan(0)
            .WithMessage("Income amount must be bigger than 0");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide income currency")
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.MadeOn)
            .NotEmpty()
            .WithMessage("Please provide income's made on date");
    }
}
