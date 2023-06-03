using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

internal class ConfirmUserRegistrationCommandValidator : Validator<ConfirmUserRegistrationCommand>
{
    public ConfirmUserRegistrationCommandValidator()
    {
        RuleFor(c => c.UserRegistrationId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide user registration ID");
        
        RuleFor(c => c.ConfirmationCode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please provide confirmation code");
    }
}
