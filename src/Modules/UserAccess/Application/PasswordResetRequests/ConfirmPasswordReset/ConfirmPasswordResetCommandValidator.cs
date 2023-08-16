using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;

internal class ConfirmPasswordResetCommandValidator : Validator<ConfirmPasswordResetCommand>
{
    public ConfirmPasswordResetCommandValidator()
    {
        RuleFor(c => c.PasswordResetRequestId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide password reset request ID");

        RuleFor(c => c.ConfirmationCode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide confirmation code");
    }
}
