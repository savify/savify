using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

internal class RegisterNewUserCommandValidator : Validator<RegisterNewUserCommand>
{
    public RegisterNewUserCommandValidator()
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
        
        RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your name");
        
        RuleFor(c => c.PreferredLanguage)
            .NotNull()
            .NotEmpty()
            .WithMessage("Preferred language field is required");
    }
}
