using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Users.FinanceTrackingSettings;

public class UserFinanceTrackingSettingsEntityTypeConfiguration : IEntityTypeConfiguration<CreateUserFinanceTrackingSettings>
{
    public void Configure(EntityTypeBuilder<CreateUserFinanceTrackingSettings> builder)
    {
        builder.ToTable("user_finance_tracking_settings");

        builder.HasKey(x => x.Id);
        builder.Property<UserId>(x => x.UserId);
        builder.OwnsOneCurrency("DefaultCurrency");
    }
}
