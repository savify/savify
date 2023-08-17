using App.BuildingBlocks.Application.Validators;
using App.Modules.Wallets.Application.Validation;
using FluentValidation;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.EditCashWallet;

internal class EditCashWalletCommandValidator : Validator<EditCashWalletCommand>
{
    public EditCashWalletCommandValidator()
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
