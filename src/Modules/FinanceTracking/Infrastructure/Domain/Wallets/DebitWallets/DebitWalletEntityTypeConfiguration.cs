using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
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
        builder.Property<int>("_balance");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime?>("_updatedAt");
        builder.Property<DateTime?>("_removedAt");
        builder.Property<bool>("_isRemoved");

        builder.ComplexProperty<Currency>("_currency", b =>
        {
            b.Property(c => c.Value).HasColumnName("currency");
        });

        builder.OwnsOne<BankAccountConnection?>("_bankAccountConnection", b =>
        {
            b.Property(c => c.Id).HasColumnName("bank_connection_id");
            b.Property(c => c.BankAccountId).HasColumnName("bank_account_id");
        });
    }
}
