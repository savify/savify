namespace App.Modules.Banks.Domain;

public record Country(string Code)
{
    public static readonly Country FakeCountry = new("XF");

    public bool IsFake() => this.Code == FakeCountry.Code;
}
