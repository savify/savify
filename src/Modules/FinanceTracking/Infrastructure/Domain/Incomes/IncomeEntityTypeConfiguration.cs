using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Incomes;

public class IncomeEntityTypeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable("incomes");
        builder.HasKey(e => e.Id);

        builder.Property<UserId>(e => e.UserId);
        builder.Property<WalletId>("_targetWalletId");
        builder.Property<CategoryId>("_categoryId");
        builder.OwnsOneMoney("_amount");
        builder.Property<DateTime>("_madeOn");
        builder.Property<string>("_comment");

        builder.PrimitiveCollection<string[]>("_tags");
    }
}
