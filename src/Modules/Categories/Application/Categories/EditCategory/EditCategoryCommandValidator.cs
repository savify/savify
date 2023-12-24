using App.BuildingBlocks.Application.Validators;
using FluentValidation;

namespace App.Modules.Categories.Application.Categories.EditCategory;

internal class EditCategoryCommandValidator : Validator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
    {
        RuleFor(c => c.Title)
            .MinimumLength(3)
            .WithMessage("Category title length should contain minimum 3 characters");

        RuleFor(c => c.IconUrl)
            .Must(BeAValidUrl)
            .WithMessage("Icon URL value is not a valid URL");
    }
}
