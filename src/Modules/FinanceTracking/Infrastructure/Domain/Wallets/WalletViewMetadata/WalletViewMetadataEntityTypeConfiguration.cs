using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadataEntityTypeConfiguration : IEntityTypeConfiguration<Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata>
{
    public void Configure(EntityTypeBuilder<Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata> builder)
    {
        builder.ToTable("wallet_view_metadata");

        builder.HasKey(x => x.WalletId);
        builder.Property(x => x.WalletId).HasColumnName("wallet_id");

        builder.Property(x => x.Color).HasColumnName("color");
        builder.Property(x => x.Icon).HasColumnName("icon");
        builder.Property(x => x.IsConsideredInTotalBalance).HasColumnName("is_considered_in_total_balance");
    }
}
