namespace App.Modules.Categories.Domain.Categories;

public record CategoryType(string Value)
{
    public static CategoryType Income => new(nameof(Income));

    public static CategoryType Expense => new(nameof(Expense));
}
