using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.EditCashWallet;

internal class EditCashWalletCommandValidator : Validator<EditCashWalletCommand>
{
    public EditCashWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide wallet title");

        RuleFor(c => c.Color)
            .MustMatchAColorHexFormat();

        RuleFor(c => c.Icon)
            .Must(BeAValidUrl)
            .WithMessage("Icon value is not a valid URL");
    }
}
