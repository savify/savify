using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios.Assets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.InvestmentPortfolios;

internal class InvestmentPortfolioEntityTypeConfiguration : IEntityTypeConfiguration<InvestmentPortfolio>
{
    public void Configure(EntityTypeBuilder<InvestmentPortfolio> builder)
    {
        builder.ToTable("investment_portfolios");

        builder.HasKey(x => x.Id);

        builder.Property<string>("_title");

        builder.OwnsMany<Asset>("_assets", y =>
        {
            y.WithOwner().HasForeignKey("investment_portfolio_id");
            y.ToTable("investment_portfolio_assets");

            y.HasKey("Id");

            y.Property<string>("_title");
            y.Property<decimal>("_amount");
            y.Property<string>("_tickerSymbol");
            y.Property<string>("_exchange");
            y.Property<string>("_country");
            y.Property<DateTime?>("_purchasedAt");

            y.OwnsOneMoney("_purchasePrice", "purchase_price_amount", "purchase_price_currency");
        });
    }
}
