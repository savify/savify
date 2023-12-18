using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Validation.Currency;
internal static class CurrencyFormatValidation
{
    public static IRuleBuilderOptions<T, string> CurrencyCodeFormatIso<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.Length(3).WithMessage("Currency should be provided in currency code format (ISO 4217)");
}
