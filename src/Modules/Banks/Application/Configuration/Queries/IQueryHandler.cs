using App.Modules.Banks.Application.Contracts;
using MediatR;

namespace App.Modules.Banks.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
