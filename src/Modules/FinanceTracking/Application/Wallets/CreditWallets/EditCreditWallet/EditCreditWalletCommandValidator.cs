using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;

internal class EditCreditWalletCommandValidator : Validator<EditCreditWalletCommand>
{
    public EditCreditWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide wallet title");

        RuleFor(c => c.CreditLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Credit limit should be greater or equal to zero");

        RuleFor(c => c.AvailableBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Available balance should be greater or equal to zero")
            .LessThanOrEqualTo(cc => cc.CreditLimit)
            .WithMessage("Available balance should be less than or equal to credit limit");

        RuleFor(c => c.Color)
            .MustMatchAColorHexFormat();

        RuleFor(c => c.Icon)
            .Must(BeAValidUrl)
            .WithMessage("Icon value is not a valid URL");
    }
}
