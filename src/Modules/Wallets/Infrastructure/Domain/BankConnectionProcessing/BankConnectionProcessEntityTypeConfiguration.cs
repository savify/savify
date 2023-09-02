using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing;

public class BankConnectionProcessEntityTypeConfiguration : IEntityTypeConfiguration<BankConnectionProcess>
{
    public void Configure(EntityTypeBuilder<BankConnectionProcess> builder)
    {
        builder.ToTable("bank_connection_processes", "wallets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<BankId>("BankId").HasColumnName("bank_id");
        builder.Property<WalletId>("WalletId").HasColumnName("wallet_id");
        builder.Property<string>("_redirectUrl").HasColumnName("redirect_url");
        builder.Property<DateTime>("_initiatedAt").HasColumnName("initiated_at");
        builder.Property<DateTime?>("_updatedAt").HasColumnName("updated_at");
        builder.Property<DateTime?>("_expiresAt").HasColumnName("expires_at");

        builder.OwnsOne<WalletType>("_walletType", b =>
        {
            b.Property(x => x.Value).HasColumnName("wallet_type");
        });

        builder.OwnsOne<BankConnectionProcessStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });
    }
}
