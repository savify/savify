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

        builder.Property<List<UserRole>>("_roles")
            .HasPostgresArrayConversion(
                userRole => userRole.Value, 
                value => UserRole.From(value))
            .HasColumnName("roles");
    }
}
