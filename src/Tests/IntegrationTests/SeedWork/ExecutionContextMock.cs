using App.BuildingBlocks.Application;

namespace App.IntegrationTests.SeedWork;

public class ExecutionContextMock : IExecutionContextAccessor
{
    public ExecutionContextMock(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }

    public bool IsAvailable => true;

    public Guid CorrelationId { get; private set; }

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }

    public void SetCorrelationId(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}
