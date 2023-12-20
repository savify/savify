using App.BuildingBlocks.Application;

namespace App.IntegrationTests.SeedWork;

public class ExecutionContextMock(Guid userId) : IExecutionContextAccessor
{
    public Guid UserId { get; private set; } = userId;

    public bool IsAvailable => true;

    public Guid CorrelationId => Guid.NewGuid();
}
