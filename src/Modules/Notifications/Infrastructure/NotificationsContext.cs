using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.Notifications.Infrastructure.Inbox;
using App.Modules.Notifications.Infrastructure.InternalCommands;
using App.Modules.Notifications.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure;

public class NotificationsContext : DbContext
{
    public DbSet<OutboxMessage>? OutboxMessages { get; set; }
    
    public DbSet<InboxMessage>? InboxMessages { get; set; }
    
    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public NotificationsContext(DbContextOptions<NotificationsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
