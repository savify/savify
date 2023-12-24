using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Infrastructure.Domain.Banks;
using App.Modules.Banks.Infrastructure.Domain.Banks.BankRevisions;
using App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure;

public class BanksContext(DbContextOptions<BanksContext> options) : DbContext(options)
{
    public required DbSet<Bank> Banks { get; set; }

    public required DbSet<BankRevision> BankRevisions { get; set; }

    public required DbSet<BanksSynchronisationProcess> BankSynchronisationProcesses { get; set; }

    public required DbSet<OutboxMessage> OutboxMessages { get; set; }

    public required DbSet<InboxMessage> InboxMessages { get; set; }

    public required DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new BankEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankRevisionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BanksSynchronisationProcessEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
