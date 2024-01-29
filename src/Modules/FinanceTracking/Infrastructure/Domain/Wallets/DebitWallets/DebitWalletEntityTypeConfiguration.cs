using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.DebitWallets;

internal class DebitWalletEntityTypeConfiguration : IEntityTypeConfiguration<DebitWallet>
{
    public void Configure(EntityTypeBuilder<DebitWallet> builder)
    {
        builder.ToTable("debit_wallets");

        builder.HasKey(w => w.Id);

        builder.Property<UserId>(w => w.UserId);
        builder.Property<string>("_title");
        builder.Property<int>("_initialBalance");
        builder.Property<bool>("_isRemoved");

        builder.OwnsOneCurrency(w => w.Currency);

        builder.OwnsOne<BankAccountConnection>("_bankAccountConnection", b =>
        {
            b.Property(c => c.Id).HasColumnName("bank_connection_id");
            b.Property(c => c.BankAccountId).HasColumnName("bank_account_id");
        });

        builder.OwnsMany<ManualBalanceChange>("_manualBalanceChanges", b =>
        {
            b.WithOwner().HasForeignKey("WalletId");
            b.ToTable("debit_wallet_manual_balance_changes");

            b.OwnsOneMoney("Amount", "amount", "currency");
            b.Property(c => c.MadeOn);

            b.OwnsOne<ManualBalanceChangeType>(c => c.Type, tb =>
            {
                tb.Property<string>(t => t.Value).HasColumnName("type");
            });
        });
    }
}
