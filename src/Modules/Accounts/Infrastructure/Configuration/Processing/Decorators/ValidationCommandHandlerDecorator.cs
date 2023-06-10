using App.BuildingBlocks.Application.Exceptions;
using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Application.Contracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing.Decorators;

internal class ValidationCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    
    private readonly IEnumerable<IValidator<T>> _validators;
    
    private readonly IStringLocalizer _localizer;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated, 
        IEnumerable<IValidator<T>> validators, 
        IStringLocalizer localizer)
    {
        _decorated = decorated;
        _validators = validators;
        _localizer = localizer;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var errors = _validators
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
                
                fieldErrors.Add(_localizer[error.ErrorMessage]);
                errorList[error.PropertyName] = fieldErrors;
            }
            
            throw new InvalidCommandException(errorList);
        }

        return await _decorated.Handle(command, cancellationToken);
    }
}
