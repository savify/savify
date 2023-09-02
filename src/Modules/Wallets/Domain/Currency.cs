namespace App.Modules.Wallets.Domain;

public record Currency(string Value)
{
    public static Currency From(string value)
    {
        return new Currency(value);
    }
}
