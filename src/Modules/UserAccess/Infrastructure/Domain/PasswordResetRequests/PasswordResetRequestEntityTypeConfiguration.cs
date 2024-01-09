using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.UserAccess.Infrastructure.Domain.PasswordResetRequests;

public class PasswordResetRequestEntityTypeConfiguration : IEntityTypeConfiguration<PasswordResetRequest>
{
    public void Configure(EntityTypeBuilder<PasswordResetRequest> builder)
    {
        builder.ToTable("password_reset_requests");

        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id);
        builder.Property<UserId>(b => b.UserId);

        builder.Property<string>("_userEmail");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime>("_validTill");
        builder.Property<DateTime?>("_confirmedAt");

        builder.ComplexProperty<ConfirmationCode>("_confirmationCode", b =>
        {
            b.Property(x => x.Value).HasColumnName("confirmation_code");
        });

        builder.ComplexProperty<PasswordResetRequestStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });
    }
}
