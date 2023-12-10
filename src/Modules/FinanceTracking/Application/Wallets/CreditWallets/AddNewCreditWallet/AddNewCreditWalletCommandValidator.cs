using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;

internal class AddNewCreditWalletCommandValidator : Validator<AddNewCreditWalletCommand>
{
    public AddNewCreditWalletCommandValidator()
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

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide wallet currency")
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.Color)
            .NotEmpty()
            .WithMessage("Please provide wallet color")
            .MustMatchAColorHexFormat();

        RuleFor(c => c.Icon)
            .NotEmpty()
            .WithMessage("Please provide wallet icon URL")
            .Must(BeAValidUrl)
            .WithMessage("Icon value is not a valid URL");
    }
}
