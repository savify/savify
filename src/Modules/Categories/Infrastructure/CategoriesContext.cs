using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.Categories.Application.Configuration.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Categories.Infrastructure;

public class CategoriesContext(DbContextOptions<CategoriesContext> options) : DbContext(options)
{
    public required DbSet<OutboxMessage> OutboxMessages { get; set; }

    public required DbSet<InboxMessage> InboxMessages { get; set; }

    public required DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
