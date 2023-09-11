using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Domain.Wallets.DebitWallets;

internal class DebitWalletEntityTypeConfiguration : IEntityTypeConfiguration<DebitWallet>
{
    public void Configure(EntityTypeBuilder<DebitWallet> builder)
    {
        builder.ToTable("debit_wallets", "wallets");

        builder.HasKey(wallet => wallet.Id);
        builder.Property(wallet => wallet.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<int>("_balance").HasColumnName("balance");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime?>("_updatedAt").HasColumnName("updated_at");
        builder.Property<DateTime?>("_removedAt").HasColumnName("removed_at");
        builder.Property<bool>("_isRemoved").HasColumnName("is_removed");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(currency => currency.Value).HasColumnName("currency");
        });

        builder.OwnsOne<BankAccountConnection?>("_bankAccountConnection", b =>
        {
            b.Property(connection => connection.Id).HasColumnName("bank_connection_id");
            b.Property(connection => connection.BankAccountId).HasColumnName("bank_account_id");
        });
    }
}
