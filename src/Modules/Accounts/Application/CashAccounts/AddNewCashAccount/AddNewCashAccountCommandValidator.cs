﻿using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.Accounts.Application.CashAccounts.AddNewCashAccount;
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
            .Length(3)
            .WithMessage("Currency should be provided in currency code format (ISO 4217)");
    }
}
