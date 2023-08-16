using App.BuildingBlocks.Application.Validators;
using App.Modules.Wallets.Application.Validation;
using FluentValidation;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;

internal class AddNewDebitWalletCommandValidator : Validator<AddNewDebitWalletCommand>
{
    public AddNewDebitWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide wallet title");

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
