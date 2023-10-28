using App.Modules.Transactions.Application.Contracts;
using MediatR;

namespace App.Modules.Transactions.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
