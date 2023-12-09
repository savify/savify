using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CreditWallets;

internal class CreditWalletsEntityTypeConfiguration : IEntityTypeConfiguration<CreditWallet>
{
    public void Configure(EntityTypeBuilder<CreditWallet> builder)
    {
        builder.ToTable("credit_wallets");

        builder.HasKey(wallet => wallet.Id);
        builder.Property(wallet => wallet.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<int>("_availableBalance").HasColumnName("available_balance");
        builder.Property<int>("_creditLimit").HasColumnName("credit_limit");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime?>("_updatedAt").HasColumnName("updated_at");
        builder.Property<DateTime?>("_removedAt").HasColumnName("removed_at");
        builder.Property<bool>("_isRemoved").HasColumnName("is_removed");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(currency => currency.Value).HasColumnName("currency");
        });
    }
}
