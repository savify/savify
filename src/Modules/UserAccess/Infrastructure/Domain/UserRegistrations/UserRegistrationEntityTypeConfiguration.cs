using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.UserAccess.Infrastructure.Domain.UserRegistrations;

public class UserRegistrationEntityTypeConfiguration : IEntityTypeConfiguration<UserRegistration>
{
    public void Configure(EntityTypeBuilder<UserRegistration> builder)
    {
        builder.ToTable("user_registrations");

        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id);

        builder.Property<string>("_email");
        builder.Property<string>("_password");
        builder.Property<string>("_name");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime>("_validTill");
        builder.Property<DateTime?>("_confirmedAt");

        builder.ComplexProperty<ConfirmationCode>("_confirmationCode", b =>
        {
            b.Property(x => x.Value).HasColumnName("confirmation_code");
        });

        builder.ComplexProperty<UserRegistrationStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });

        builder.ComplexProperty<Country>("_country", b =>
        {
            b.Property(x => x.Value).HasColumnName("country");
        });

        builder.ComplexProperty<Language>("_preferredLanguage", b =>
        {
            b.Property(x => x.Value).HasColumnName("preferred_language");
        });
    }
}
