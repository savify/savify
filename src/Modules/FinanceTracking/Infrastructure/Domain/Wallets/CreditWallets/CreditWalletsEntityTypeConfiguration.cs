using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
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
        builder.Property<int>("_availableBalance");
        builder.Property<int>("_creditLimit");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime?>("_updatedAt");
        builder.Property<DateTime?>("_removedAt");
        builder.Property<bool>("_isRemoved");

        builder.OwnsOneCurrency("_currency");
    }
}
