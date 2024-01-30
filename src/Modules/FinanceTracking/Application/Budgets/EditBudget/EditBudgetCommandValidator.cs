using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Currencies;
using App.Modules.FinanceTracking.Application.Validation.Currency;
using App.Modules.FinanceTracking.Domain.Categories;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Budgets.EditBudget;

public class EditBudgetCommandValidator : Validator<EditBudgetCommand>
{
    public EditBudgetCommandValidator(ICurrenciesProvider currenciesProvider, ICategoryRepository categoryRepository)
    {
        RuleFor(c => c.Currency)
            .MustBeInCurrencyCodeIsoFormat()
            .MustAsync(async (currencyCode, _) =>
            {
                var supportedCurrencyCodes = (await currenciesProvider.GetCurrenciesAsync()).Select(c => c.Value).ToArray();

                return supportedCurrencyCodes.Contains(currencyCode);

            })
            .WithMessage("Given currency code is not supported");

        RuleFor(c => c.CategoriesBudget)
            .Must(categoriesBudget => !categoriesBudget.Any(c => c.Value < 0))
            .WithMessage("Budget amount for each category cannot be negative")
            .MustAsync(async (categoriesBudget, _) =>
            {
                foreach (var categoryId in categoriesBudget.Keys)
                {
                    var existsWithId = await categoryRepository.ExistsWithIdAsync(new CategoryId(categoryId));
                    if (!existsWithId) return false;
                }

                return true;
            })
            .WithMessage("Some of provided categories does not exist");
    }

}
