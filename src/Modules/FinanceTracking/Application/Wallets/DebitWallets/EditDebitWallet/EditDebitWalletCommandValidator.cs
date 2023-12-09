using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;

internal class EditDebitWalletCommandValidator : Validator<EditDebitWalletCommand>
{
    public EditDebitWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .MinimumLength(3)
            .WithMessage("Wallet title length should contain minimum 3 characters");

        RuleFor(c => c.Currency)
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.Color)
            .MustMatchAColorHexFormat();

        RuleFor(c => c.Icon)
            .Must(BeAValidUrl)
            .WithMessage("Icon value is not a valid URL");
    }
}
