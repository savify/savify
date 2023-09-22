using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

public class SynchroniseBanksCommand : CommandBase<BanksSynchronisationResultDto>
{
    public Guid UserId { get; }

    public SynchroniseBanksCommand(Guid userId)
    {
        UserId = userId;
    }
}
