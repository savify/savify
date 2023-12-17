namespace App.Modules.Categories.Domain;

public record Url(string Value)
{
    public static Url From(string value) => new(value);
}
