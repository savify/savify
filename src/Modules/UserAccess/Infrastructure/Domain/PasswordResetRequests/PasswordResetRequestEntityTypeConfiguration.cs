using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.UserAccess.Infrastructure.Domain.PasswordResetRequests;

public class PasswordResetRequestEntityTypeConfiguration : IEntityTypeConfiguration<PasswordResetRequest>
{
    public void Configure(EntityTypeBuilder<PasswordResetRequest> builder)
    {
        builder.ToTable("password_reset_requests", "user_access");

        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id).HasColumnName("id");

        builder.Property<string>("_userEmail").HasColumnName("user_email");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime>("_validTill").HasColumnName("valid_till");
        builder.Property<DateTime?>("_confirmedAt").HasColumnName("confirmed_at");

        builder.OwnsOne<ConfirmationCode>("_confirmationCode", b =>
        {
            b.Property(x => x.Value).HasColumnName("confirmation_code");
        });

        builder.OwnsOne<PasswordResetRequestStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });
    }
}
