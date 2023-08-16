using App.BuildingBlocks.Application.Validators;
using App.Modules.Wallets.Application.Validation;
using App.Modules.Wallets.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;

internal class AddNewCashWalletCommandValidator : Validator<AddNewCashWalletCommand>
{
    public AddNewCashWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide wallet title");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide wallet currency")
            .MustMatchCurrencyCodeISOFormat();

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
