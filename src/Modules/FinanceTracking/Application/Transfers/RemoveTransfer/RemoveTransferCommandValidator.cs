using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;

internal class RemoveTransferCommandValidator : Validator<RemoveTransferCommand>
{
    public RemoveTransferCommandValidator()
    {
        RuleFor(c => c.TransferId)
            .NotEmpty()
            .WithMessage("Please provide transfer id");
    }
}
