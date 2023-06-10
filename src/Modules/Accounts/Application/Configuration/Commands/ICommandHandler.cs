using App.Modules.Accounts.Application.Contracts;
using MediatR;

namespace App.Modules.Accounts.Application.Configuration.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand { }

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult> { }
