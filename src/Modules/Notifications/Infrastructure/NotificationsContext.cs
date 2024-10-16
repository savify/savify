using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.Notifications.Application.Configuration.Data;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure;

public class NotificationsContext(DbContextOptions<NotificationsContext> options) : DbContext(options)
{
    public required DbSet<UserNotificationSettings> UserNotificationSettings { get; set; }

    public required DbSet<InboxMessage> InboxMessages { get; set; }

    public required DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new UserNotificationSettingsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
