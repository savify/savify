using App.BuildingBlocks.Application.Validators;
using App.Modules.Accounts.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.Accounts.Application.Accounts.CashAccounts.AddNewCashAccount;
internal class AddNewCashAccountCommandValidator : Validator<AddNewCashAccountCommand>
{
    public AddNewCashAccountCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide the new cash account title");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide the new cash account currenty")
            .CurrencyCodeFormatISO();
    }
}
