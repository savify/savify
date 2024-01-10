namespace App.BuildingBlocks.Application;

public interface IExecutionContextAccessor
{
    Guid UserId { get; }

    string AccessToken { get; }

    Guid CorrelationId { get; }

    bool IsAvailable { get; }
}
