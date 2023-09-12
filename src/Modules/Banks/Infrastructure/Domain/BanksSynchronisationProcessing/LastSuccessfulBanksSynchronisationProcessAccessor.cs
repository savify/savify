using App.BuildingBlocks.Application.Data;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using Dapper;

namespace App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;

public class LastSuccessfulBanksSynchronisationProcessAccessor : ILastSuccessfulBanksSynchronisationProcessAccessor
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public LastSuccessfulBanksSynchronisationProcessAccessor(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<ILastSuccessfulBanksSynchronisationProcessAccessor.LastSuccessfulBanksSynchronisationProcess?> AccessAsync()
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = @"SELECT id, status, finished_at FROM banks.banks_synchronisation_processes p 
                                WHERE p.status = @status ORDER BY p.finished_at DESC";

        return await connection.QueryFirstOrDefaultAsync<ILastSuccessfulBanksSynchronisationProcessAccessor.LastSuccessfulBanksSynchronisationProcess>(
            sql,
            new { status = BanksSynchronisationProcessStatus.Finished.Value });
    }
}
