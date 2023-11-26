using App.Modules.Notifications.Domain.UserNotificationSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;

public class UserNotificationSettingsEntityTypeConfiguration : IEntityTypeConfiguration<Notifications.Domain.UserNotificationSettings.UserNotificationSettings>
{
    public void Configure(EntityTypeBuilder<Notifications.Domain.UserNotificationSettings.UserNotificationSettings> builder)
    {
        builder.ToTable("user_notification_settings");

        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("Email").HasColumnName("email");
        builder.Property<string>("Name").HasColumnName("name");

        builder.OwnsOne<Language>("PreferredLanguage", b =>
        {
            b.Property(x => x.Value).HasColumnName("preferred_language");
        });
    }
}
