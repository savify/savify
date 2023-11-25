using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.Outbox;

public interface IOutbox<TContext> where TContext : DbContext
{
    void Add(OutboxMessage message);
}
