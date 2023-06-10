using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.Accounts.Infrastructure.Inbox;
using App.Modules.Accounts.Infrastructure.InternalCommands;
using App.Modules.Accounts.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure;

public class AccountsContext : DbContext
{
    public DbSet<OutboxMessage>? OutboxMessages { get; set; }
    
    public DbSet<InboxMessage>? InboxMessages { get; set; }
    
    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
