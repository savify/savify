using App.BuildingBlocks.Application.Exceptions;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Localization;
using App.Modules.Banks.Application.Contracts;
using FluentValidation;
using Microsoft.Extensions.Localization;
using static App.Modules.Banks.Infrastructure.Configuration.Processing.Decorators.ValidationCommandHandlerDecorator;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Decorators;

internal class ValidationCommandHandlerDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IEnumerable<IValidator<T>> validators,
    ILocalizerProvider localizerProvider)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    private readonly IStringLocalizer _localizer = localizerProvider.GetLocalizer();

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        Validate(command, validators, _localizer);

        return await decorated.Handle(command, cancellationToken);
    }
}

internal class ValidationCommandHandlerDecorator<T>(
    ICommandHandler<T> decorated,
    IEnumerable<IValidator<T>> validators,
    ILocalizerProvider localizerProvider)
    : ICommandHandler<T>
    where T : ICommand
{
    private readonly IStringLocalizer _localizer = localizerProvider.GetLocalizer();

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        Validate(command, validators, _localizer);

        await decorated.Handle(command, cancellationToken);
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
