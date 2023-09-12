namespace App.Modules.Banks.Domain.BankSynchronisationProcessing;

public record BankSynchronisationProcessStatus(string Value)
{
    public static BankSynchronisationProcessStatus Started = new(nameof(Started));

    public static BankSynchronisationProcessStatus Finished = new(nameof(Finished));

    public static BankSynchronisationProcessStatus Failed = new(nameof(Failed));
}
