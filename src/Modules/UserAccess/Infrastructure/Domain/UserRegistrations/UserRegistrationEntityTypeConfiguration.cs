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
        builder.ToTable("user_registrations", "user_access");

        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id).HasColumnName("id");

        builder.Property<string>("_email").HasColumnName("email");
        builder.Property<string>("_password").HasColumnName("password");
        builder.Property<string>("_name").HasColumnName("name");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime>("_validTill").HasColumnName("valid_till");
        builder.Property<DateTime?>("_confirmedAt").HasColumnName("confirmed_at");

        builder.OwnsOne<ConfirmationCode>("_confirmationCode", b =>
        {
            b.Property(x => x.Value).HasColumnName("confirmation_code");
        });

        builder.OwnsOne<UserRegistrationStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });

        builder.OwnsOne<Country>("_country", b =>
        {
            b.Property(x => x.Value).HasColumnName("country");
        });

        builder.OwnsOne<Language>("_preferredLanguage", b =>
        {
            b.Property(x => x.Value).HasColumnName("preferred_language");
        });
    }
}
