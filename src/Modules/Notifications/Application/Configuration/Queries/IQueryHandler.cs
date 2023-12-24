using App.Modules.Notifications.Application.Contracts;
using MediatR;

namespace App.Modules.Notifications.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>;
