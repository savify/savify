using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.AccountViewMetadata;

public class AccountViewMetadataEntityTypeConfiguration : IEntityTypeConfiguration<Modules.Accounts.Domain.Accounts.AccountViewMetadata.AccountViewMetadata>
{
    public void Configure(EntityTypeBuilder<Modules.Accounts.Domain.Accounts.AccountViewMetadata.AccountViewMetadata> builder)
    {
        builder.ToTable("account_view_metadata", "accounts");
        
        builder.HasKey(x => x.AccountId);
        builder.Property(x => x.AccountId).HasColumnName("account_id");
        
        builder.Property(x => x.Color).HasColumnName("color");
        builder.Property(x => x.Icon).HasColumnName("icon");
        builder.Property(x => x.IsConsideredInTotalBalance).HasColumnName("is_considered_in_total_balance");
    }
}
