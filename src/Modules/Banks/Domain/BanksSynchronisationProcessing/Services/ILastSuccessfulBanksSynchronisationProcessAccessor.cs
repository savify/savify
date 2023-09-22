namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

public interface ILastSuccessfulBanksSynchronisationProcessAccessor
{
    public Task<LastSuccessfulBanksSynchronisationProcess?> AccessAsync();

    public class LastSuccessfulBanksSynchronisationProcess
    {
        public BanksSynchronisationProcessId Id { get; }

        public BanksSynchronisationProcessStatus Status { get; }

        public DateTime FinishedAt { get; }

        public LastSuccessfulBanksSynchronisationProcess(Guid id, string status, DateTime finishedAt)
        {
            Id = new BanksSynchronisationProcessId(id);
            Status = new BanksSynchronisationProcessStatus(status);
            FinishedAt = finishedAt;
        }
    }
}
