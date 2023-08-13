using App.Modules.Wallets.Application.Contracts;
using MediatR;

namespace App.Modules.Wallets.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
