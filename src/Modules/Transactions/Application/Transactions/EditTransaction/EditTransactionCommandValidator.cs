using App.BuildingBlocks.Application.Validators;
using App.Modules.Transactions.Application.Validation;
using FluentValidation;

namespace App.Modules.Transactions.Application.Transactions.EditTransaction;

internal class EditTransactionCommandValidator : Validator<EditTransactionCommand>
{
    public EditTransactionCommandValidator()
    {
        RuleFor(c => c.Source)
            .NotEmpty()
            .WithMessage("Please provide a transaction source");

        RuleFor(c => c.Source.SenderAddress)
            .NotEmpty()
            .WithMessage("Please provider a source sender address");

        RuleFor(c => c.Source.Amount)
            .GreaterThan(0)
            .WithMessage("Source transaction amount should be greater than 0");

        RuleFor(c => c.Source.Currency)
            .NotEmpty()
            .WithMessage("Please provide source currency")
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.Target.RecipientAddress)
            .NotEmpty()
            .WithMessage("Please provider a target recipient address");

        RuleFor(c => c.Target.Amount)
            .GreaterThan(0)
            .WithMessage("Target transaction amount should be greater than 0");

        RuleFor(c => c.Target.Currency)
            .NotEmpty()
            .WithMessage("Please provide target currency")
            .MustMatchCurrencyCodeIsoFormat();

        RuleFor(c => c.Comment)
            .NotNull()
            .WithMessage("Comment should have any value or be empty");

        RuleFor(c => c.Tags)
            .NotNull()
            .WithMessage("Tags collection should have items or be empty");
    }
}
