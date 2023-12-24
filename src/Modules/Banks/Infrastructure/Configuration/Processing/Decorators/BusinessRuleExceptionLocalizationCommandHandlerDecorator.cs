using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Localization;
using App.Modules.Banks.Application.Contracts;
using Microsoft.Extensions.Localization;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Decorators;

internal class BusinessRuleExceptionLocalizationCommandHandlerDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    ILocalizerProvider localizerProvider)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    private readonly IStringLocalizer _localizer = localizerProvider.GetLocalizer();

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        try
        {
            return await decorated.Handle(command, cancellationToken);
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

internal class BusinessRuleExceptionLocalizationCommandHandlerDecorator<T>(
    ICommandHandler<T> decorated,
    ILocalizerProvider localizerProvider)
    : ICommandHandler<T>
    where T : ICommand
{
    private readonly IStringLocalizer _localizer = localizerProvider.GetLocalizer();

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        try
        {
            await decorated.Handle(command, cancellationToken);
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
