using App.BuildingBlocks.Application.Validators;
using App.Modules.Wallets.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;

internal class AddNewCashWalletCommandValidator : Validator<AddNewCashWalletCommand>
{
    public AddNewCashWalletCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide the new cash wallet title");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide the new cash wallet currency")
            .CurrencyCodeFormatISO();
    }
}
