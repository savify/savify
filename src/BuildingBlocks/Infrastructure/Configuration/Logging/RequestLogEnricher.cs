using App.BuildingBlocks.Application;
using Serilog.Core;
using Serilog.Events;

namespace App.BuildingBlocks.Infrastructure.Configuration.Logging;

public class RequestLogEnricher(IExecutionContextAccessor executionContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (executionContextAccessor.IsAvailable)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(executionContextAccessor.CorrelationId)));
        }
    }
}
