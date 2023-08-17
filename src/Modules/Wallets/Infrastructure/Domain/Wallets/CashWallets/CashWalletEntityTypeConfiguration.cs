using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CashWallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Domain.Wallets.CashWallets;

internal class CashWalletEntityTypeConfiguration : IEntityTypeConfiguration<CashWallet>
{
    public void Configure(EntityTypeBuilder<CashWallet> builder)
    {
        builder.ToTable("cash_wallets", "wallets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<int>("_balance").HasColumnName("balance");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime?>("_updatedAt").HasColumnName("updated_at");
        builder.Property<DateTime?>("_removedAt").HasColumnName("removed_at");
        builder.Property<bool>("_isRemoved").HasColumnName("is_removed");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(x => x.Value).HasColumnName("currency");
        });
    }
}
