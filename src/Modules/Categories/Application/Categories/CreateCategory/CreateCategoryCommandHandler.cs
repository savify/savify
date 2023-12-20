using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Domain.Categories;

namespace App.Modules.Categories.Application.Categories.CreateCategory;

internal class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ICategoriesCounter categoriesCounter)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.Create(command.ExternalId, command.Title, CategoryType.From(command.Type), categoriesCounter);

        await categoryRepository.AddAsync(category);

        return category.Id.Value;
    }
}
