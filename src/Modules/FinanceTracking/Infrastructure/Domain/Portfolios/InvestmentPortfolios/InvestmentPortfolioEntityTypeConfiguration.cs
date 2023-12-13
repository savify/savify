using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios.Assets;
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

            y.OwnsOne<Money>("_purchasePrice", m =>
            {
                m.Property(x => x.Amount).HasColumnName("purchase_price_amount").HasColumnType("money");
                m.OwnsOne(x => x.Currency, c =>
                {
                    c.Property(x => x.Value).HasColumnName("purchase_price_currency");
                });
            });
        });
    }
}
