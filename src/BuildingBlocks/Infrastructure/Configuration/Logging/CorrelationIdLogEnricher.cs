using Serilog.Core;
using Serilog.Events;

namespace App.BuildingBlocks.Infrastructure.Configuration.Logging;

public class CorrelationIdLogEnricher(Guid correlationId) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(correlationId)));
    }
}
