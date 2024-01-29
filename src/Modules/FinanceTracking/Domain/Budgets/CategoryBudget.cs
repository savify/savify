using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Budgets;

public record CategoryBudget
{
    public CategoryId CategoryId { get; private set; }

    public Money Amount { get; private set; }

    public static CategoryBudget From(CategoryId categoryId, Money amount)
    {
        return new CategoryBudget(categoryId, amount);
    }

    private CategoryBudget(CategoryId categoryId, Money amount)
    {
        CategoryId = categoryId;
        Amount = amount;
    }

    private CategoryBudget() { }
}
