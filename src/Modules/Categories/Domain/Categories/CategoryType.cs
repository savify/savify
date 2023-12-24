namespace App.Modules.Categories.Domain.Categories;

public record CategoryType(string Value)
{
    public static CategoryType Income => new(nameof(Income));

    public static CategoryType Expense => new(nameof(Expense));

    public static CategoryType From(string value)
    {
        return value switch
        {
            nameof(Income) => Income,
            nameof(Expense) => Expense,
            _ => throw new ArgumentException("Invalid category type")
        };
    }

    public static IEnumerable<string> GetAllValues()
    {
        yield return Income.Value;
        yield return Expense.Value;
    }
}
