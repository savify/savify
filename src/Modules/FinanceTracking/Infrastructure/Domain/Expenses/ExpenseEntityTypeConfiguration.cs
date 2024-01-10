using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Expenses;

public class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("expenses");
        builder.HasKey(e => e.Id);

        builder.Property<UserId>(e => e.UserId);
        builder.Property<WalletId>("_sourceWalletId");
        builder.Property<CategoryId>("_categoryId");
        builder.OwnsOneMoney("_amount");
        builder.Property<DateTime>("_madeOn");
        builder.Property<string>("_comment");

        builder.PrimitiveCollection<string[]>("_tags");
    }
}
