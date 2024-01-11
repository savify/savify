using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

public class SynchroniseBanksCommand(Guid userId) : CommandBase<BanksSynchronisationResultDto>
{
    public Guid UserId { get; } = userId;
}
