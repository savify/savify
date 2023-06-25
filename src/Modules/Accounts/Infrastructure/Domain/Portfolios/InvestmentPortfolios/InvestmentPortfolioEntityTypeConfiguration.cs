using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Investments.InvestmentPortfolios;
using App.Modules.Accounts.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Accounts.Infrastructure.Domain.Portfolios.InvestmentPortfolios;

internal class InvestmentPortfolioEntityTypeConfiguration : IEntityTypeConfiguration<InvestmentPortfolio>
{
    public void Configure(EntityTypeBuilder<InvestmentPortfolio> builder)
    {
        builder.ToTable("investment_portfolios", "accounts");

        builder.HasKey(portfolio => portfolio.Id);
        builder.Property(portfolio => portfolio.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(currency => currency.Value).HasColumnName("currency");
        })
    }
}
