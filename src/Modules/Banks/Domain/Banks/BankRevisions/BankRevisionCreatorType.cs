namespace App.Modules.Banks.Domain.Banks.BankRevisions;

public record BankRevisionCreatorType(string Value)
{
    public static BankRevisionCreatorType User => new(nameof(User));

    public static BankRevisionCreatorType SynchronisationProcess => new(nameof(SynchronisationProcess));
}
