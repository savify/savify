using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.Accounts.Application.CreditAccounts.AddNewCreditAccount;

internal class AddNewCreditAccountCommandValidator : Validator<AddNewCreditAccountCommand>
{
    public AddNewCreditAccountCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Please provide the new credit account title");

        RuleFor(c => c.AvailableBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("AvailableBalance should be greater or equal to zero");

        RuleFor(c => c.CreditLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CreditLimit should be greated or equal to zero")
            .GreaterThanOrEqualTo(cc => cc.AvailableBalance)
            .WithMessage("Credit limit should be greater or equal to AvailableBalance");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide the new cash account currenty")
            .Length(3)
            .WithMessage("Currency should be provided in currency code format (ISO 4217).");
    }
}
