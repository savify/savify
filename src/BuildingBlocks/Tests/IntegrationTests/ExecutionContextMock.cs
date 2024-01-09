using App.BuildingBlocks.Application;

namespace App.BuildingBlocks.Tests.IntegrationTests;

public class ExecutionContextMock(Guid userId) : IExecutionContextAccessor
{
    public Guid UserId { get; } = userId;

    public string AccessToken { get; } = "access_token";

    public bool IsAvailable => true;

    public Guid CorrelationId { get; } = Guid.NewGuid();
}
