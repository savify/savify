using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.Transactions.Domain.Transactions;
using App.Modules.Transactions.Infrastructure.Domain.Transactions;
using App.Modules.Transactions.Infrastructure.Inbox;
using App.Modules.Transactions.Infrastructure.InternalCommands;
using App.Modules.Transactions.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Transactions.Infrastructure;

public class TransactionsContext : DbContext
{
    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public TransactionsContext(DbContextOptions<TransactionsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(TransactionsModule.DatabaseSchemaName);

        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionsEntityTypeConfiguration());
    }
}
