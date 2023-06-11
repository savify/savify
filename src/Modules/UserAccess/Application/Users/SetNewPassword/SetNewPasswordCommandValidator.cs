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

        RuleFor(c => c.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your password")
            .MinimumLength(8).WithMessage("Your password must contain at least 8 characters")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one special character (!? *.)");
    }
}
