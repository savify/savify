using App.BuildingBlocks.Application.Validators;
using App.Modules.Accounts.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.Accounts.Application.Accounts.DebitAccounts.AddNewDebitAccount;

internal class AddNewDebitAccountCommandValidator : Validator<AddNewDebitAccountCommand>
{
    public AddNewDebitAccountCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide the new debit account title");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide the new debit account currenty")
            .CurrencyCodeFormatISO();
    }
}
