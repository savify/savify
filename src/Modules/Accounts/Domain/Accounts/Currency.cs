namespace App.Modules.Accounts.Domain.Accounts;

public record Currency(string Value)
{
    public static Currency From(string value)
    {
        return new Currency(value);
    }
}
