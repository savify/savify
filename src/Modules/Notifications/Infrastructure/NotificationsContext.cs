using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Inbox;
using App.Modules.Notifications.Infrastructure.InternalCommands;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure;

public class NotificationsContext : DbContext
{
    public DbSet<UserNotificationSettings>? UserNotificationSettings { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public NotificationsContext(DbContextOptions<NotificationsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserNotificationSettingsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
