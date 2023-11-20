using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.Categories.Application.Configuration.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Categories.Infrastructure;

public class CategoriesContext : DbContext
{
    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public CategoriesContext(DbContextOptions<CategoriesContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
