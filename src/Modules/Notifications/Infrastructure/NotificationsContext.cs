using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Inbox;
using App.Modules.Notifications.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure;

public class NotificationsContext : DbContext
{
    public DbSet<UserNotificationSettings>? UserNotificationSettings { get; set; }

    public DbSet<OutboxMessage>? OutboxMessages { get; set; }
    
    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public NotificationsContext(DbContextOptions<NotificationsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserNotificationSettingsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
    }
}
