using App.BuildingBlocks.Application;
using App.BuildingBlocks.Infrastructure.Configuration.Logging;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Infrastructure.Configuration.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.Decorators;

internal class LoggingCommandHandlerDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    ILoggerProvider loggerProvider,
    IExecutionContextAccessor executionContextAccessor)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        using (LogContext.Push(
                   new RequestLogEnricher(executionContextAccessor),
                   new CommandLogEnricher(command)))
        {
            try
            {
                _logger.Information("Executing command {@Command}", command);

                var result = await decorated.Handle(command, cancellationToken);

                _logger.Information("Command processed successful, result {Result}", result);

                return result;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Command processing failed; {Message}", exception.Message);
                throw;
            }
        }
    }

    private class CommandLogEnricher(ICommand<TResult> command) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{command.Id.ToString()}")));
        }
    }
}

internal class LoggingCommandHandlerDecorator<T>(
    ICommandHandler<T> decorated,
    ILoggerProvider loggerProvider,
    IExecutionContextAccessor executionContextAccessor)
    : ICommandHandler<T>
    where T : ICommand
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        if (command is IRecurringCommand)
        {
            await decorated.Handle(command, cancellationToken);
            return;
        }

        var enrichers = new List<ILogEventEnricher>();
        enrichers.Add(new CommandLogEnricher(command));
        enrichers.Add(new RequestLogEnricher(executionContextAccessor));

        if (command is InternalCommandBase internalCommand)
        {
            enrichers.Add(new CorrelationIdLogEnricher(internalCommand.CorrelationId));
        }

        using (LogContext.Push(enrichers.ToArray()))
        {
            try
            {
                _logger.Information("Executing command {@Command}", command);

                await decorated.Handle(command, cancellationToken);

                _logger.Information("Command processed successful");
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Command processing failed; {Message}", exception.Message);
                throw;
            }
        }
    }

    private class CommandLogEnricher(ICommand command) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{command.Id.ToString()}")));
        }
    }
}
