using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Configuration.Localization;
using App.Modules.FinanceTracking.Application.Contracts;
using FluentValidation;
using Microsoft.Extensions.Localization;
using static App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators.ValidationCommandHandlerDecorator;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators;

internal class ValidationCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;

    private readonly IEnumerable<IValidator<T>> _validators;

    private readonly IStringLocalizer _localizer;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        IEnumerable<IValidator<T>> validators,
        ILocalizerProvider localizerProvider)
    {
        _decorated = decorated;
        _validators = validators;
        _localizer = localizerProvider.GetLocalizer();
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        Validate(command, _validators, _localizer);

        return await _decorated.Handle(command, cancellationToken);
    }
}

internal class ValidationCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;

    private readonly IEnumerable<IValidator<T>> _validators;

    private readonly IStringLocalizer _localizer;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IEnumerable<IValidator<T>> validators,
        ILocalizerProvider localizerProvider)
    {
        _decorated = decorated;
        _validators = validators;
        _localizer = localizerProvider.GetLocalizer();
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        Validate(command, _validators, _localizer);

        await _decorated.Handle(command, cancellationToken);
    }
}

internal static class ValidationCommandHandlerDecorator
{
    internal static void Validate<T>(T command, IEnumerable<IValidator<T>> validators, IStringLocalizer localizer)
    {
        var errors = validators
            .Select(v => v.Validate(command))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
        {
            var errorList = new Dictionary<string, List<string>>();

            foreach (var error in errors)
            {
                var fieldErrors = new List<string>();

                if (errorList.ContainsKey(error.PropertyName))
                {
                    fieldErrors = errorList[error.PropertyName];
                }

                fieldErrors.Add(localizer[error.ErrorMessage]);
                errorList[error.PropertyName] = fieldErrors;
            }

            throw new InvalidCommandException(errorList);
        }
    }
}
