using App.BuildingBlocks.Application.Validators;
using App.Modules.UserAccess.Domain.Users;
using FluentValidation;

namespace App.Modules.UserAccess.Application.Users.CreateNewUser;

internal class CreateNewUserCommandValidator : Validator<CreateNewUserCommand>
{
    public CreateNewUserCommandValidator()
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

        RuleFor(c => c.Role)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide Your role")
            .Must(role => UserRole.GetAvailableRolesValues().Contains(role))
            .WithMessage("Provided role must be one of the following: 'Admin', 'User'");


        RuleFor(c => c.Country)
            .NotNull()
            .NotEmpty()
            .WithMessage("Country field is required")
            .Matches(@"^[A-Z]{2}$")
            .WithMessage("Country should contain exactly 2 uppercase letters");
    }
}
