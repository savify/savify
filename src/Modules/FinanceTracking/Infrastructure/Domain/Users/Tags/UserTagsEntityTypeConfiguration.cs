using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Users.Tags;

internal class UserTagsEntityTypeConfiguration : IEntityTypeConfiguration<FinanceTracking.Domain.Users.Tags.UserTags>
{
    public void Configure(EntityTypeBuilder<FinanceTracking.Domain.Users.Tags.UserTags> builder)
    {
        builder.HasKey(p => p.UserId);
        builder.PrimitiveCollection<List<string>>("_tags");
    }
}
