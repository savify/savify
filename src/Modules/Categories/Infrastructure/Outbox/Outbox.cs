using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.Categories.Infrastructure.Outbox;

public class Outbox : IOutbox
{
    private readonly CategoriesContext _categoriesContext;

    public Outbox(CategoriesContext categoriesContext)
    {
        _categoriesContext = categoriesContext;
    }

    public void Add(OutboxMessage message)
    {
        _categoriesContext.OutboxMessages?.Add(message);
    }
}
