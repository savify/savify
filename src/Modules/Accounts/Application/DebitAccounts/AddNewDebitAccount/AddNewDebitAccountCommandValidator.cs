using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.Accounts.Application.DebitAccounts.AddNewDebitAccount;

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
            .Length(3)
            .WithMessage("Currency should be provided in currency code format (ISO 4217)");
    }
}
