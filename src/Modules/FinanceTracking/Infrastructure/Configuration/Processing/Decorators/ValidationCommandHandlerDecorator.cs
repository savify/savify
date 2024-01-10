using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Configuration.Localization;
using App.Modules.FinanceTracking.Application.Contracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using static App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators.ValidationCommandHandlerDecorator;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators;

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
        await Validate(command, validators, _localizer);

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
        await Validate(command, validators, _localizer);

        await decorated.Handle(command, cancellationToken);
    }
}

internal static class ValidationCommandHandlerDecorator
{
    internal static async Task Validate<T>(T command, IEnumerable<IValidator<T>> validators, IStringLocalizer localizer)
    {
        var validationResults = new List<ValidationResult>();

        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(command);
            validationResults.Add(validationResult);
        }

        var errors = validationResults
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
