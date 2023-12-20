using App.BuildingBlocks.Application;

namespace App.BuildingBlocks.Tests.IntegrationTests;

public class ExecutionContextMock(Guid userId) : IExecutionContextAccessor
{
    public Guid UserId { get; } = userId;

    public bool IsAvailable => true;

    public Guid CorrelationId { get; } = Guid.NewGuid();
}
