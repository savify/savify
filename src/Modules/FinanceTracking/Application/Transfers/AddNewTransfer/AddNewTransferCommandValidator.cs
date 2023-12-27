using App.BuildingBlocks.Application.Validators;
using App.Modules.FinanceTracking.Application.Validation;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class AddNewTransferCommandValidator : Validator<AddNewTransferCommand>
{
    public AddNewTransferCommandValidator()
    {
        RuleFor(c => c.SourceWalletId)
            .NotEmpty()
            .WithMessage("Please provide source wallet id");

        RuleFor(c => c.TargetWalletId)
            .NotEmpty()
            .WithMessage("Please provide target wallet id");

        RuleFor(c => c.Amount)
            .GreaterThan(0)
            .WithMessage("Transfer amount must be bigger than 0");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Please provide transfer currency")
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.MadeOn)
            .NotEmpty()
            .WithMessage("Please provide transfer's made on date");
    }
}
