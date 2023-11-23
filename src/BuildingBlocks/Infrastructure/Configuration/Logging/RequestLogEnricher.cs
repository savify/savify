using App.BuildingBlocks.Application;
using Serilog.Core;
using Serilog.Events;

namespace App.BuildingBlocks.Infrastructure.Configuration.Logging;

public class RequestLogEnricher : ILogEventEnricher
{
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (_executionContextAccessor.IsAvailable)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
        }
    }
}
