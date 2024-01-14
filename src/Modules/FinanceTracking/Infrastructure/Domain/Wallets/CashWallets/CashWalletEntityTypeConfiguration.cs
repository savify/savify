using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;
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
        builder.Property<int>("_initialBalance");
        builder.Property<bool>("_isRemoved");

        builder.OwnsOneCurrency(w => w.Currency);

        builder.OwnsMany<ManualBalanceChange>("_manualBalanceChanges", b =>
        {
            b.WithOwner().HasForeignKey("WalletId");
            b.ToTable("cash_wallet_manual_balance_changes");

            b.OwnsOneMoney("Amount", "amount", "currency");
            b.Property(c => c.MadeOn);

            b.OwnsOne<ManualBalanceChangeType>(c => c.Type, tb =>
            {
                tb.Property<string>(t => t.Value).HasColumnName("type");
            });
        });
    }
}
