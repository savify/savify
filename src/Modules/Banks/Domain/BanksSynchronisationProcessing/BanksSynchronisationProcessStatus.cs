namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public record BanksSynchronisationProcessStatus(string Value)
{
    public static readonly BanksSynchronisationProcessStatus Started = new(nameof(Started));

    public static readonly BanksSynchronisationProcessStatus Finished = new(nameof(Finished));

    public static readonly BanksSynchronisationProcessStatus Failed = new(nameof(Failed));
}
