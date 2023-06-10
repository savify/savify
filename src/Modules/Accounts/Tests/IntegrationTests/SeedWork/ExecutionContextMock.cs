using App.BuildingBlocks.Application;

namespace App.Modules.Accounts.IntegrationTests.SeedWork;

public class ExecutionContextMock : IExecutionContextAccessor
{
    public ExecutionContextMock(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }
    
    public bool IsAvailable { get; }
    
    public Guid CorrelationId { get; }
    
    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }
}
