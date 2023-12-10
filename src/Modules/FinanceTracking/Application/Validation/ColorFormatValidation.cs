using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Validation;

internal static class ColorFormatValidation
{
    public static IRuleBuilderOptions<T, string?> MustMatchAColorHexFormat<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Matches(@"^#?([0-9a-f]{6}|[0-9a-f]{3})$").WithMessage("Color must be provided in HEX color format (e.g. #FFFFFF)");
}
