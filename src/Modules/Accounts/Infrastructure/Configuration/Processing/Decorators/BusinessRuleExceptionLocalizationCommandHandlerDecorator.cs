using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Application.Contracts;
using Microsoft.Extensions.Localization;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing.Decorators;

internal class BusinessRuleExceptionLocalizationCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;

    private readonly IStringLocalizer _localizer;

    public BusinessRuleExceptionLocalizationCommandHandlerDecorator(ICommandHandler<T, TResult> decorated, IStringLocalizer localizer)
    {
        _decorated = decorated;
        _localizer = localizer;
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
