using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.Users.SetNewPassword;

internal class SetNewPasswordCommandValidator : Validator<SetNewPasswordCommand>
{
    public SetNewPasswordCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide user ID");

        // TODO: add more validations for strong password
        RuleFor(c => c.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your password");
    }
}
