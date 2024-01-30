using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Currencies;
using App.Modules.FinanceTracking.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Budgets.AddBudget;

public class AddBudgetCommandValidator : Validator<AddBudgetCommand>
{
    public AddBudgetCommandValidator(ICurrenciesProvider currenciesProvider)
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
            .Must(categoriesBudget => !categoriesBudget.Any(c => c.Value < 0));
    }

}
