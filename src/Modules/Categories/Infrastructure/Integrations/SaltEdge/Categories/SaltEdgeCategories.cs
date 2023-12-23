namespace App.Modules.Categories.Infrastructure.Integrations.SaltEdge.Categories;

public class SaltEdgeCategories(IDictionary<string, IList<string>> business, IDictionary<string, IList<string>> personal)
{
    public const string Income = "income";

    public IDictionary<string, IList<string>> Business { get; set; } = business;

    public IDictionary<string, IList<string>> Personal { get; set; } = personal;

    public static SaltEdgeCategories Empty() => new(
        new Dictionary<string, IList<string>>(),
        new Dictionary<string, IList<string>>());
}
