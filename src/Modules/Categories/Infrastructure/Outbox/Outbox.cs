using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.Categories.Infrastructure.Outbox;

public class Outbox(CategoriesContext categoriesContext) : IOutbox<CategoriesContext>
{
    public void Add(OutboxMessage message)
    {
        categoriesContext.OutboxMessages.Add(message);
    }
}
