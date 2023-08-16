using App.BuildingBlocks.Application.Validators;
using App.Modules.Wallets.Application.Validation;
using App.Modules.Wallets.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;

internal class AddNewDebitWalletCommandValidator : Validator<AddNewDebitWalletCommand>
{
    public AddNewDebitWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide the new debit wallet title");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide the new debit wallet currency")
            .MustMatchCurrencyCodeISOFormat();
        
        RuleFor(c => c.Color)
            .NotEmpty()
            .WithMessage("Please provide the new debit wallet color")
            .MustMatchAColorHexFormat();

        RuleFor(c => c.Icon)
            .NotEmpty()
            .WithMessage("Please provide the new debit wallet icon URL")
            .Must(BeAValidUrl)
            .WithMessage("Icon value is not a valid URL");
    }
}
