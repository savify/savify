using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Domain.Portfolios.InvestmentPortfolios;

internal class InvestmentPortfolioEntityTypeConfiguration : IEntityTypeConfiguration<InvestmentPortfolio>
{
    public void Configure(EntityTypeBuilder<InvestmentPortfolio> builder)
    {
        builder.ToTable("investment_portfolios", "wallets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<string>("_title").HasColumnName("title");

        builder.OwnsMany<Asset>("_assets", y =>
        {
            y.WithOwner().HasForeignKey("investment_portfolio_id");
            y.ToTable("investment_portfolio_assets", "wallets");

            y.HasKey("Id");
            y.Property<AssetId>("Id").HasColumnName("id");

            y.Property<string>("_title").HasColumnName("title");
            y.Property<decimal>("_amount").HasColumnName("amount");
            y.Property<string>("_tickerSymbol").HasColumnName("ticker_symbol");
            y.Property<string>("_exchange").HasColumnName("exchange");
            y.Property<string>("_country").HasColumnName("country");
            y.Property<DateTime?>("_purchasedAt").HasColumnName("purchased_at");

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
