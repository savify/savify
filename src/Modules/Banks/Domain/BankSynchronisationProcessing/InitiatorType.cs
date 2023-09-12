namespace App.Modules.Banks.Domain.BankSynchronisationProcessing;

public record InitiatorType(string Type)
{
    public static InitiatorType InternalCommand = new(nameof(InternalCommand));

    public static InitiatorType User = new(nameof(User));
}
