namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public record BanksSynchronisationProcessStatus(string Value)
{
    public static BanksSynchronisationProcessStatus Started = new(nameof(Started));

    public static BanksSynchronisationProcessStatus Finished = new(nameof(Finished));

    public static BanksSynchronisationProcessStatus Failed = new(nameof(Failed));
}
