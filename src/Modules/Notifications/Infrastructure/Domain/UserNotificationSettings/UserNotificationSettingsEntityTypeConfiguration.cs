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

        builder.Property<UserId>(x => x.UserId);
        builder.Property<string>(x => x.Email);
        builder.Property<string>(x => x.Name);

        builder.ComplexProperty<Language>(x => x.PreferredLanguage, b =>
        {
            b.Property(x => x.Value).HasColumnName("preferred_language");
        });
    }
}
