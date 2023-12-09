using App.BuildingBlocks.Application;
using App.BuildingBlocks.Infrastructure.Configuration.Logging;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators;

internal class LoggingCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;

    private readonly ILogger _logger;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public LoggingCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        ILoggerProvider loggerProvider,
        IExecutionContextAccessor executionContextAccessor)
    {
        _decorated = decorated;
        _logger = loggerProvider.GetLogger();
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        using (LogContext.Push(
                   new RequestLogEnricher(_executionContextAccessor),
                   new CommandLogEnricher(command)))
        {
            try
            {
                _logger.Information("Executing command {@Command}", command);

                var result = await _decorated.Handle(command, cancellationToken);

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

    private class CommandLogEnricher : ILogEventEnricher
    {
        private readonly ICommand<TResult> _command;

        public CommandLogEnricher(ICommand<TResult> command)
        {
            _command = command;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
        }
    }
}

internal class LoggingCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;

    private readonly ILogger _logger;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public LoggingCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        ILoggerProvider loggerProvider,
        IExecutionContextAccessor executionContextAccessor)
    {
        _decorated = decorated;
        _logger = loggerProvider.GetLogger();
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        if (command is IRecurringCommand)
        {
            await _decorated.Handle(command, cancellationToken);
            return;
        }

        var enrichers = new List<ILogEventEnricher>();
        enrichers.Add(new CommandLogEnricher(command));
        enrichers.Add(new RequestLogEnricher(_executionContextAccessor));

        if (command is InternalCommandBase internalCommand)
        {
            enrichers.Add(new CorrelationIdLogEnricher(internalCommand.CorrelationId));
        }

        using (LogContext.Push(enrichers.ToArray()))
        {
            try
            {
                _logger.Information("Executing command {@Command}", command);

                await _decorated.Handle(command, cancellationToken);

                _logger.Information("Command processed successful");
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Command processing failed; {Message}", exception.Message);
                throw;
            }
        }
    }

    private class CommandLogEnricher : ILogEventEnricher
    {
        private readonly ICommand _command;

        public CommandLogEnricher(ICommand command)
        {
            _command = command;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
        }
    }
}
