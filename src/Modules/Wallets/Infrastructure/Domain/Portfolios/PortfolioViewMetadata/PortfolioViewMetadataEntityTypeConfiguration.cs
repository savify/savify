using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.Portfolios.PortfolioViewMetadata;

internal class PortfolioViewMetadataEntityTypeConfiguration : IEntityTypeConfiguration<Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata> builder)
    {
        builder.ToTable("portfolio_view_matadata");

        builder.HasKey(x => x.PortfolioId);
        builder.Property(x => x.PortfolioId).HasColumnName("portfolio_id");

        builder.Property(x => x.Color).HasColumnName("color");
        builder.Property(x => x.Icon).HasColumnName("icon");
        builder.Property(x => x.IsConsideredInTotalBalance).HasColumnName("is_considered_in_total_balance");
    }
}
