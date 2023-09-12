namespace App.Modules.Banks.Domain;

public record Country(string Code)
{
    public static readonly Country FakeCountry = new("XF");

    public static Country From(string code) => new(code);

    public bool IsFake() => this.Code == FakeCountry.Code;
}
