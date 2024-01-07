using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;

public class WalletHistoryEntityTypeConfiguration : IEntityTypeConfiguration<WalletHistory>
{
    public void Configure(EntityTypeBuilder<WalletHistory> builder)
    {
        builder.ToTable("wallet_histories");

        builder.HasKey(h => h.WalletId);

        builder.OwnsMany<WalletHistoryEvent>(h => h.Events, b =>
        {
            b.WithOwner().HasForeignKey("WalletHistoryId");
            b.ToTable("wallet_history_events");

            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property<string>(e => e.Type);
            b.Property<string>(e => e.Data);
        });
    }
}
