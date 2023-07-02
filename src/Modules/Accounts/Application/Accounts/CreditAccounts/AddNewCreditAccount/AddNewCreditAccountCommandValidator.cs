using App.BuildingBlocks.Application.Validators;
using App.Modules.Accounts.Application.Validation.Currency;
using FluentValidation;

namespace App.Modules.Accounts.Application.Accounts.CreditAccounts.AddNewCreditAccount;

internal class AddNewCreditAccountCommandValidator : Validator<AddNewCreditAccountCommand>
{
    public AddNewCreditAccountCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide the new credit account title");

        RuleFor(c => c.CreditLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CreditLimit should be greated or equal to zero")
            .GreaterThanOrEqualTo(cc => cc.AvailableBalance)
            .WithMessage("Credit limit should be greater or equal to available balance");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide the new credit account currenty")
            .CurrencyCodeFormatISO();
    }
}
