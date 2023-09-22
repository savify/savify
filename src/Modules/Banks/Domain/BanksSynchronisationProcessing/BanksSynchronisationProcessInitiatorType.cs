namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public record BanksSynchronisationProcessInitiatorType(string Value)
{
    public static BanksSynchronisationProcessInitiatorType InternalCommand = new(nameof(InternalCommand));

    public static BanksSynchronisationProcessInitiatorType User = new(nameof(User));
}
