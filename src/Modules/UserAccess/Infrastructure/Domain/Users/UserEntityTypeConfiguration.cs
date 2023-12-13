using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id);

        builder.Property<string>("_email");
        builder.Property<string>("_password");
        builder.Property<string>("_name");
        builder.Property<bool>("_isActive");
        builder.Property<DateTime>("_createdAt");

        builder.OwnsMany<UserRole>("_roles", b =>
        {
            b.WithOwner().HasForeignKey("UserId");
            b.ToTable("user_roles");
            b.Property<UserId>("UserId");
            b.Property<string>("Value").HasColumnName("role_code");
            b.HasKey("UserId", "Value");
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
