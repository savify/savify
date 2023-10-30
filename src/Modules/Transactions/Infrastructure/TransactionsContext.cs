using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Transactions.Infrastructure;

public class TransactionsContext : DbContext
{
    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public TransactionsContext(DbContextOptions<TransactionsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration(TransactionsModule.DatabaseSchemaName));
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration(TransactionsModule.DatabaseSchemaName));
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration(TransactionsModule.DatabaseSchemaName));
    }
}
