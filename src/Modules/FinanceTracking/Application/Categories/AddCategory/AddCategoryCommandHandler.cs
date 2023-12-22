using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;

namespace App.Modules.FinanceTracking.Application.Categories.AddCategory;

public class AddCategoryCommandHandler(ICategoryRepository categoryRepository) : ICommandHandler<AddCategoryCommand>
{
    public Task Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category(new CategoryId(command.CategoryId), command.ExternalCategoryId);

        return categoryRepository.AddAsync(category);
    }
}
