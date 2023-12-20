using App.Modules.Categories.Application.Contracts;
using MediatR;

namespace App.Modules.Categories.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>;
