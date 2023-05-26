using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

public class CreateNewUserCommandValidator : Validator<CreateNewUserCommand>
{
    public CreateNewUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your email address")
            .Must(BeAValidEmail)
            .WithMessage("Provided value is not a valid email address");

        // TODO: add more validations for strong password
        RuleFor(c => c.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your password");
    }
}
