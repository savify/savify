using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "user_access");
        
        builder.HasKey(x => x.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        
        builder.Property<string>("_email").HasColumnName("email");
        builder.Property<string>("_password").HasColumnName("password");
        builder.Property<string>("_name").HasColumnName("name");
        builder.Property<bool>("_isActive").HasColumnName("is_active");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");

        builder.OwnsMany<UserRole>("_roles", b =>
        {
            b.WithOwner().HasForeignKey("UserId");
            b.ToTable("user_roles");
            b.Property<UserId>("UserId").HasColumnName("user_id");
            b.Property<string>("Value").HasColumnName("role_code");
            b.HasKey("UserId", "Value");
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
