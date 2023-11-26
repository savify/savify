using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Application.Configuration.Localization;
using App.Modules.Transactions.Application.Contracts;
using Microsoft.Extensions.Localization;

namespace App.Modules.Transactions.Infrastructure.Configuration.Processing.Decorators;

internal class BusinessRuleExceptionLocalizationCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;

    private readonly IStringLocalizer _localizer;

    public BusinessRuleExceptionLocalizationCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        ILocalizerProvider localizerProvider)
    {
        _decorated = decorated;
        _localizer = localizerProvider.GetLocalizer();
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        try
        {
            return await _decorated.Handle(command, cancellationToken);
        }
        catch (BusinessRuleValidationException exception)
        {
            var localizedMessage = _localizer[
                exception.BrokenRule.MessageTemplate,
                exception.BrokenRule.MessageArguments ?? Array.Empty<object>()
            ];

            throw new LocalizedBusinessRuleValidationException(exception.BrokenRule, localizedMessage);
        }
    }
}

internal class BusinessRuleExceptionLocalizationCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;

    private readonly IStringLocalizer _localizer;

    public BusinessRuleExceptionLocalizationCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        ILocalizerProvider localizerProvider)
    {
        _decorated = decorated;
        _localizer = localizerProvider.GetLocalizer();
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        try
        {
            await _decorated.Handle(command, cancellationToken);
        }
        catch (BusinessRuleValidationException exception)
        {
            var localizedMessage = _localizer[
                exception.BrokenRule.MessageTemplate,
                exception.BrokenRule.MessageArguments ?? Array.Empty<object>()
            ];

            throw new LocalizedBusinessRuleValidationException(exception.BrokenRule, localizedMessage);
        }
    }
}
