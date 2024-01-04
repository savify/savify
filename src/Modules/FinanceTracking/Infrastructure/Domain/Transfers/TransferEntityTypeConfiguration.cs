using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Transfers;

internal class TransferEntityTypeConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.ToTable("transfers");
        builder.HasKey(t => t.Id);

        builder.Property<UserId>(t => t.UserId);
        builder.Property<WalletId>("_sourceWalletId");
        builder.Property<WalletId>("_targetWalletId");
        builder.OwnsOneMoney("_amount");
        builder.Property<DateTime>("_madeOn");
        builder.Property<string>("_comment");

        builder.PrimitiveCollection<string[]>("_tags");
    }
}
