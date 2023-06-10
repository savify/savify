using App.Modules.Accounts.Application.Contracts;
using MediatR;

namespace App.Modules.Accounts.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
