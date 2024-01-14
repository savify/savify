using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class AddNewTransferCommandValidator : Validator<AddNewTransferCommand>
{
    public AddNewTransferCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("Please provide user id");

        RuleFor(c => c.SourceWalletId)
            .NotEmpty()
            .WithMessage("Please provide source wallet id");

        RuleFor(c => c.TargetWalletId)
            .NotEmpty()
            .WithMessage("Please provide target wallet id");

        RuleFor(c => c.SourceAmount)
            .GreaterThan(0)
            .WithMessage("Transfer amount must be bigger than 0");

        RuleFor(c => c.TargetAmount)
            .GreaterThan(0)
            .WithMessage("Transfer amount must be bigger than 0");

        RuleFor(c => c.MadeOn)
            .NotEmpty()
            .WithMessage("Please provide transfer's made on date");
    }
}
