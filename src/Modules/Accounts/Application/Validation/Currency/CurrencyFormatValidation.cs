using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Modules.Accounts.Application.Validation.Currency;
internal static class CurrencyFormatValidation
{
    public static IRuleBuilderOptions<T, string> CurrencyCodeFormatISO<T>(this IRuleBuilder<T, string> ruleBuilder) => 
        ruleBuilder.Length(3).WithMessage("Currency should be provided in currency code format (ISO 4217)");
}
