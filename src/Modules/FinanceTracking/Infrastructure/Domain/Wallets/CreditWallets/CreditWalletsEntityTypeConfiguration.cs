using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CreditWallets;

internal class CreditWalletsEntityTypeConfiguration : IEntityTypeConfiguration<CreditWallet>
{
    public void Configure(EntityTypeBuilder<CreditWallet> builder)
    {
        builder.ToTable("credit_wallets");

        builder.HasKey(w => w.Id);

        builder.Property<UserId>(w => w.UserId);
        builder.Property<string>("_title");
        builder.Property<int>("_initialAvailableBalance");
        builder.Property<int>("_creditLimit");
        builder.Property<bool>("_isRemoved");

        builder.OwnsOneCurrency(w => w.Currency);

        builder.OwnsMany<ManualBalanceChange>("_manualBalanceChanges", b =>
        {
            b.WithOwner().HasForeignKey("WalletId");
            b.ToTable("credit_wallet_manual_balance_changes");

            b.OwnsOneMoney("Amount", "amount", "currency");
            b.Property(c => c.MadeOn);

            b.OwnsOne<ManualBalanceChangeType>(c => c.Type, tb =>
            {
                tb.Property<string>(t => t.Value).HasColumnName("type");
            });
        });
    }
}
