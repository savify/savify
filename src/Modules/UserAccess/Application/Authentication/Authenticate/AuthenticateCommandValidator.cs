using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.Authentication.Authenticate;

internal class AuthenticateCommandValidator : Validator<AuthenticateCommand>
{
    public AuthenticateCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email address is required to authenticate");
        
        RuleFor(c => c.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required to authenticate");
    }
}
