using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Domain;
using App.Modules.Categories.Domain.Categories;

namespace App.Modules.Categories.Application.Categories.EditCategory;

internal class EditCategoryCommandHandler(ICategoryRepository categoryRepository) : ICommandHandler<EditCategoryCommand>
{
    public async Task Handle(EditCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(new CategoryId(command.CategoryId));

        category.Edit(command.Title, command.IconUrl is not null ? Url.From(command.IconUrl) : null);
    }
}
