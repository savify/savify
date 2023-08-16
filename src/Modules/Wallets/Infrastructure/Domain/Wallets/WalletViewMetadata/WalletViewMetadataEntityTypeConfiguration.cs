using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadataEntityTypeConfiguration : IEntityTypeConfiguration<Modules.Wallets.Domain.Wallets.WalletViewMetadata.WalletViewMetadata>
{
    public void Configure(EntityTypeBuilder<Modules.Wallets.Domain.Wallets.WalletViewMetadata.WalletViewMetadata> builder)
    {
        builder.ToTable("wallet_view_metadata", "wallets");

        builder.HasKey(x => x.WalletId);
        builder.Property(x => x.WalletId).HasColumnName("wallet_id");

        builder.Property(x => x.Color).HasColumnName("color");
        builder.Property(x => x.Icon).HasColumnName("icon");
        builder.Property(x => x.IsConsideredInTotalBalance).HasColumnName("is_considered_in_total_balance");
    }
}
