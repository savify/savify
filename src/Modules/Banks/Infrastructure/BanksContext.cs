using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Infrastructure.Domain.Banks;
using App.Modules.Banks.Infrastructure.Domain.Banks.BankRevisions;
using App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure;

public class BanksContext : DbContext
{
    public DbSet<Bank>? Banks { get; set; }

    public DbSet<BankRevision>? BankRevisions { get; set; }

    public DbSet<BanksSynchronisationProcess>? BankSynchronisationProcesses { get; set; }

    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public BanksContext(DbContextOptions<BanksContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BankEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankRevisionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BanksSynchronisationProcessEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration(BanksModule.DatabaseSchemaName));
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration(BanksModule.DatabaseSchemaName));
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration(BanksModule.DatabaseSchemaName));
    }
}
