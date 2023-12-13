using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing;

public class BankConnectionProcessEntityTypeConfiguration : IEntityTypeConfiguration<BankConnectionProcess>
{
    public void Configure(EntityTypeBuilder<BankConnectionProcess> builder)
    {
        builder.ToTable("bank_connection_processes");

        builder.HasKey(x => x.Id);

        builder.Property<UserId>("UserId");
        builder.Property<BankId>("BankId");
        builder.Property<WalletId>("WalletId");
        builder.Property<string>("_redirectUrl");
        builder.Property<DateTime>("_initiatedAt");
        builder.Property<DateTime?>("_updatedAt");
        builder.Property<DateTime?>("_redirectUrlExpiresAt");

        builder.ComplexProperty<WalletType>("_walletType", b =>
        {
            b.Property(x => x.Value).HasColumnName("wallet_type");
        });

        builder.ComplexProperty<BankConnectionProcessStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasConversion<string>().HasColumnName("status");
        });
    }
}
