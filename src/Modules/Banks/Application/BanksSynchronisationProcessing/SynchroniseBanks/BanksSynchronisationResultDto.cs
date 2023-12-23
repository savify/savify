namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

public class BanksSynchronisationResultDto(string status)
{
    public string Status { get; } = status;
}
