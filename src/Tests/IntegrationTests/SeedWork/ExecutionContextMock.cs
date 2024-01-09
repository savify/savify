using App.BuildingBlocks.Application;

namespace App.IntegrationTests.SeedWork;

public class ExecutionContextMock(Guid userId) : IExecutionContextAccessor
{
    public Guid UserId { get; } = userId;

    public string AccessToken { get; } = "access_token";

    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public Guid CorrelationId { get; }

    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public bool IsAvailable { get; }
}
