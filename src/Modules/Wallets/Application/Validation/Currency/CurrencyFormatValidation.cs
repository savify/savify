﻿using FluentValidation;

namespace App.Modules.Wallets.Application.Validation.Currency;
internal static class CurrencyFormatValidation
{
    public static IRuleBuilderOptions<T, string> MustMatchCurrencyCodeISOFormat<T>(this IRuleBuilder<T, string> ruleBuilder) =>
    ruleBuilder.Length(3).WithMessage("Currency should be provided in currency code format (ISO 4217)");
}
