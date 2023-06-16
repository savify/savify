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

        RuleFor(c => c.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your password")
            .MinimumLength(8).WithMessage("Your password must contain at least 8 characters")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one special character (!? *.)");
        
        RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your name");

        RuleFor(c => c.Country)
            .NotNull()
            .NotEmpty()
            .WithMessage("Country field is required")
            .Matches(@"^[A-Z]{2}$")
            .WithMessage("Country should contain exactly 2 uppercase letters");
        
        RuleFor(c => c.PreferredLanguage)
            .NotNull()
            .NotEmpty()
            .WithMessage("Preferred language field is required")
            .Matches(@"^[a-z]{2}$")
            .WithMessage("Preferred language should contain exactly 2 lowercase letters");
    }
}
