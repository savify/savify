using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.PortfolioViewMetadata;

internal class PortfolioViewMetadataEntityTypeConfiguration : IEntityTypeConfiguration<Modules.FinanceTracking.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Modules.FinanceTracking.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata> builder)
    {
        builder.ToTable("portfolio_view_metadata");

        builder.HasKey(x => x.PortfolioId);

        builder.Property(x => x.Color);
        builder.Property(x => x.Icon);
        builder.Property(x => x.IsConsideredInTotalBalance);
    }
}
