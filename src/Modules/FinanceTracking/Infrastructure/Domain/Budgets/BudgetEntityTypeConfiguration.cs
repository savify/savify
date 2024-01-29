using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Budgets;

public class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("budgets");
        builder.HasKey(x => x.Id);

        builder.Property<UserId>(e => e.UserId);

        builder.OwnsOne<BudgetPeriod>("_period", b =>
        {
            b.Property(p => p.Start);
            b.Property(p => p.End);
        }).Navigation("_period").IsRequired();

        builder.OwnsMany<CategoryBudget>("_categoriesBudget", b =>
        {
            b.WithOwner().HasForeignKey("BudgetId");
            b.ToTable("categories_budgets");

            b.Property<BudgetId>("BudgetId");
            b.Property<CategoryId>(c => c.CategoryId);
            b.OwnsOneMoney(c => c.Amount);
            b.HasKey("BudgetId", "CategoryId");
        });
    }
}
