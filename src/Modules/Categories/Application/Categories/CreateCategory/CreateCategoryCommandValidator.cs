using App.BuildingBlocks.Application.Validators;
using App.Modules.Categories.Domain.Categories;
using FluentValidation;

namespace App.Modules.Categories.Application.Categories.CreateCategory;

internal class AddCategoryCommandValidator : Validator<CreateCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(c => c.Title)
            .MinimumLength(3)
            .WithMessage("Category title length should contain minimum 3 characters");

        RuleFor(c => c.Type)
            .Must(type => CategoryType.GetAllValues().Contains(type))
            .WithMessage("Category type must be one of the following: 'Income', 'Expense'");
    }
}
