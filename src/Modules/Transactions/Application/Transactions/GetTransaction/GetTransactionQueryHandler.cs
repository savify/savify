using App.BuildingBlocks.Application.Data;
using App.Modules.Transactions.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Transactions.Application.Transactions.GetTransaction;

internal class GetTransactionQueryHandler : IQueryHandler<GetTransactionQuery, TransactionDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetTransactionQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<TransactionDto?> Handle(GetTransactionQuery query, CancellationToken cancellationToken)
    {
        using var conneciton = _sqlConnectionFactory.GetOpenConnection();

        const string sql =
            """
                SELECT t.id, t.type, t.made_on as madeOn, t.comment, t.tags, 
                       ts.sender_address AS senderAddress, ts.amount AS amount, ts.amount_currency AS currency, 
                       tt.recipient_address AS recipientAddress, tt.amount AS amount, tt.amount_currency AS currency 
                FROM transactions.transactions t 
                INNER JOIN transactions.transaction_sources ts ON ts.transaction_id = t.id 
                INNER JOIN transactions.transaction_targets tt ON tt.transaction_id = t.id 
                WHERE t.id = @TransactionId
            """;

        var transactions = await conneciton.QueryAsync<TransactionDto, SourceDto, TargetDto, TransactionDto>(sql, (transaction, source, target) =>
        {
            transaction.Source = source;
            transaction.Target = target;

            return transaction;
        },
        new { query.TransactionId },
        splitOn: "senderAddress, recipientAddress");

        return transactions.SingleOrDefault();
    }
}
