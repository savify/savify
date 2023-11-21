using App.BuildingBlocks.Application;
using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Application.Contracts;
using App.Modules.Transactions.Infrastructure.Configuration.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace App.Modules.Transactions.Infrastructure.Configuration.Processing.Decorators;

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
        _logger = loggerProvider.Provide();
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
        _logger = loggerProvider.Provide();
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        if (command is IRecurringCommand)
        {
            await _decorated.Handle(command, cancellationToken);
            return;
        }

        using (LogContext.Push(new RequestLogEnricher(_executionContextAccessor), new CommandLogEnricher(command)))
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

internal class RequestLogEnricher : ILogEventEnricher
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
