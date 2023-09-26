namespace App.Modules.Banks.Domain.Banks.BankRevisions;

public record BankRevisionType(string Value)
{
    public static BankRevisionType Added => new(nameof(Added));

    public static BankRevisionType Updated => new(nameof(Updated));
}
