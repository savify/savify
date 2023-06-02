using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

internal class RefreshTokensCommandValidator : Validator<RefreshTokensCommand>
{
    public RefreshTokensCommandValidator()
    {
        RuleFor(c => c.RefreshToken)
            .NotNull()
            .NotEmpty()
            .WithMessage("This field is required");
    }
}
