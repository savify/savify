using Serilog.Core;
using Serilog.Events;

namespace App.BuildingBlocks.Infrastructure.Configuration.Logging;

public class CorrelationIdLogEnricher : ILogEventEnricher
{
    private readonly Guid _correlationId;

    public CorrelationIdLogEnricher(Guid correlationId)
    {
        _correlationId = correlationId;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_correlationId)));
    }
}
