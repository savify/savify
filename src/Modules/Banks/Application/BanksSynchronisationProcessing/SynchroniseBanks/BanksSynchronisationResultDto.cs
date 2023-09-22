namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

public class BanksSynchronisationResultDto
{
    public string Status { get; }

    public BanksSynchronisationResultDto(string status)
    {
        Status = status;
    }
}
