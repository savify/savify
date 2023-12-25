using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CashWallets;

internal class CashWalletEntityTypeConfiguration : IEntityTypeConfiguration<CashWallet>
{
    public void Configure(EntityTypeBuilder<CashWallet> builder)
    {
        builder.ToTable("cash_wallets");

        builder.HasKey(w => w.Id);

        builder.Property<UserId>(w => w.UserId);
        builder.Property<string>("_title");
        builder.Property<int>("_balance");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime?>("_updatedAt");
        builder.Property<DateTime?>("_removedAt");
        builder.Property<bool>("_isRemoved");

        builder.OwnsOneCurrency("_currency");
    }
}
