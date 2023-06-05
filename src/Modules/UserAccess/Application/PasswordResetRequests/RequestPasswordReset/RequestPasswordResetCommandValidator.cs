using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

internal class RequestPasswordResetCommandValidator : Validator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your email address")
            .Must(BeAValidEmail)
            .WithMessage("Provided value is not a valid email address");
    }
}
