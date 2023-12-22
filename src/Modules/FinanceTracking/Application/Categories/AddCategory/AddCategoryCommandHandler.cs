using App.Modules.FinanceTracking.Application.Configuration.Commands;

namespace App.Modules.FinanceTracking.Application.Categories;

public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand>
{
    public Task Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        // TODO: add category here
    }
}
